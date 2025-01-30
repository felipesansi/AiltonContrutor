using AiltonContrutor.Context;
using AiltonContrutor.Models;
using AiltonContrutor.Repositorio.Interfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AiltonContrutor.Repositorio
{
    public class UploadFotosService : IUploadFotosService
    {
        private readonly string _appKey;
        private readonly string _appSecret;
        private readonly string _refreshToken;
        private string? _accessToken; // Access Token atualizado
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _appDbContext;

        public UploadFotosService(AppDbContext appDbContext)
        {
            // Carrega as variáveis do arquivo .env
            DotNetEnv.Env.Load();

            _appKey = Environment.GetEnvironmentVariable("DROPBOX_APP_KEY") 
                ?? throw new InvalidOperationException("DROPBOX_APP_KEY não encontrado.");
            _appSecret = Environment.GetEnvironmentVariable("DROPBOX_APP_SECRET") 
                ?? throw new InvalidOperationException("DROPBOX_APP_SECRET não encontrado.");
            _refreshToken = Environment.GetEnvironmentVariable("DROPBOX_REFRESH_TOKEN") 
                ?? throw new InvalidOperationException("DROPBOX_REFRESH_TOKEN não encontrado.");

            _httpClient = new HttpClient();
            _appDbContext = appDbContext;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string caminho_destino)
        {
            _accessToken = await ObeterTokenAcessoAsync();

            using (var dbx = new DropboxClient(_accessToken))
            {
                using (var stream = file.OpenReadStream())
                {
                    // Faz o upload do arquivo para o Dropbox
                    var Resultado_Upload = await dbx.Files.UploadAsync(
                        caminho_destino,
                        WriteMode.Overwrite.Instance,
                        body: stream);

                    // Obtém o link compartilhado para o arquivo carregado
                    var link_gerado = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(Resultado_Upload.PathDisplay);
                    return link_gerado.Url;
                }
            }
        }

        // Método para obter o token de acesso
        private async Task<string> ObeterTokenAcessoAsync()
        {
            var corpo_requisicao = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", _refreshToken }
            };

            var requisicao = new HttpRequestMessage(HttpMethod.Post, "https://api.dropbox.com/oauth2/token")
            {
                Content = new FormUrlEncodedContent(corpo_requisicao)
            };
            // Adiciona o cabeçalho de autorização
            var authHeaderValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_appKey}:{_appSecret}"));
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            // Envia a requisição
            var resposta = await _httpClient.SendAsync(requisicao);

            // Verifica se a requisição foi bem sucedida
            resposta.EnsureSuccessStatusCode();

            // Desserializa a resposta
            var respostaContent = await resposta.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResposta>(respostaContent);

            if (tokenResponse == null)
            {
                throw new InvalidOperationException("Falha ao desserializar a resposta do token.");
            }

            await SalvarTokensAsync(tokenResponse.AccessToken, _refreshToken, tokenResponse.TokenType, DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn));

            return tokenResponse.AccessToken;
        }

        public async Task SalvarTokensAsync(string accessToken, string refreshToken, string tokenType, DateTime dataExpiracao)
        {
            var tokens = await _appDbContext.DadosDropBox.FirstOrDefaultAsync();

            if (tokens == null)
            {
                tokens = new DadosDropBox
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    TokenType = tokenType,
                    DataExpiracao = dataExpiracao
                };
                _appDbContext.DadosDropBox.Add(tokens);
            }
            else
            {
                tokens.AccessToken = accessToken;
                tokens.RefreshToken = refreshToken;
                tokens.TokenType = tokenType;
                tokens.DataExpiracao = dataExpiracao;
                _appDbContext.DadosDropBox.Update(tokens);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<DadosDropBox> RecuperarTokenActivoAsync()
        {
            var token = await _appDbContext.DadosDropBox.FirstOrDefaultAsync();

            if (token == null)
            {
                throw new InvalidOperationException("Falha ao recuperar o token.");
            }
            return token;
        }
    }
}
