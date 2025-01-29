using AiltonContrutor.Context;
using AiltonContrutor.Models;
using AiltonContrutor.Repositorio.Interfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AiltonContrutor.Repositorio
{
    public class UploadFotosService : IUploadFotosService
    {
        private readonly string _appKey = "awlugc5oznmjln8"; // Sua App Key
        private readonly string _appSecret = "2mb4byq0gtxy9x0"; // Seu App Secret
        private readonly string _refreshToken; // Seu Refresh Token
        private string _accessToken; // Access Token atualizado
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _appDbContext;

        public UploadFotosService(string refreshToken, AppDbContext appDbContext)
        {
            _refreshToken = refreshToken;
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

            var authHeaderValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_appKey}:{_appSecret}"));
            requisicao.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

            var resposta = await _httpClient.SendAsync(requisicao);
            resposta.EnsureSuccessStatusCode();

            var respostaContent = await resposta.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResposta>(respostaContent);

           
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
            return await _appDbContext.DadosDropBox.FirstOrDefaultAsync();
        }
    }

   
}
