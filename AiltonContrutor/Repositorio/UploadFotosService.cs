
using AiltonContrutor.Context;
using AiltonContrutor.Models;
using AiltonContrutor.Repositorio.Interfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace AiltonContrutor.Repositorio
{
    public class UploadFotosService : IUploadFotosService
    {
        private readonly string _dropBoxToken;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<UploadFotosService> _logger;

        public UploadFotosService(string dropBoxToken, AppDbContext appDbContext, ILogger<UploadFotosService> logger)
        {
            _dropBoxToken = dropBoxToken;
            _appDbContext = appDbContext;
            _logger = logger;
        }

    


     
        public async Task<string> UploadFileAsync(IFormFile file, string caminho_destino)
        {
            
            var dadosDropbox = await RecuperarTokenActivoAsync();
            if (dadosDropbox == null)
            {
                _logger.LogError("Não foi possível encontrar um token válido.");
                return null;
            }

            using (var dbx = new DropboxClient(dadosDropbox.AccessToken))
            {
                using (var stream = file.OpenReadStream())
                {
                   
                    var resultadoUpload = await dbx.Files.UploadAsync(
                        caminho_destino,
                        WriteMode.Overwrite.Instance,
                        body: stream);

                    // Obtém o link compartilhado para o arquivo carregado
                    var linkGerado = await dbx.Sharing.CreateSharedLinkWithSettingsAsync(resultadoUpload.PathDisplay);
                    return linkGerado.Url;
                }
            }
        }

        
        public async Task<DadosDropBox> RecuperarTokenActivoAsync()
        {
            var dadosDropbox = await _appDbContext.DadosDropBox.FirstOrDefaultAsync(d => d.DataExpiracao > DateTime.Now);
            if (dadosDropbox == null)
            {
                _logger.LogWarning("Token expirado ou inválido. Tentando renovar.");
            
                await RenovarTokenAsync();
                dadosDropbox = await _appDbContext.DadosDropBox.FirstOrDefaultAsync(d => d.DataExpiracao > DateTime.Now);
            }
            return dadosDropbox;
        }

       
        private async Task RenovarTokenAsync()
        {
         
            var dadosDropbox = await _appDbContext.DadosDropBox.FirstOrDefaultAsync();
            if (dadosDropbox == null || string.IsNullOrEmpty(dadosDropbox.RefreshToken))
            {
                _logger.LogError("Não há refresh_token disponível.");
                return;
            }

           
            var clientId = "v5msre23zhfmhjw"; 
            var clientSecret = "snx45r8ftz6gti2"; 
            var refreshToken = dadosDropbox.RefreshToken;

            using (var client = new HttpClient())
            {
                var valores = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshToken },
                    { "client_id", clientId },
                    { "client_secret", clientSecret }
                };

                var content = new FormUrlEncodedContent(valores); 
                var response = await client.PostAsync("https://api.dropboxapi.com/oauth2/token", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync(); // Obtenha a resposta como uma string
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseString);

                  
                    dadosDropbox.AccessToken = tokenResponse.AccessToken;
                    dadosDropbox.DataExpiracao = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
                    _appDbContext.DadosDropBox.Update(dadosDropbox);
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    _logger.LogError("Erro ao renovar o token. Verifique o refresh_token.");
                }
            }
        }

        
        public async Task SalvarTokensAsync(string accessToken, string refreshToken, string tokenType, DateTime dataExpiracao)
        {
            try
            {
                var existeToken = await _appDbContext.DadosDropBox.FirstOrDefaultAsync(d => d.RefreshToken == refreshToken);
                if (existeToken != null)
                {
                    existeToken.AccessToken = accessToken;
                    existeToken.TokenType = tokenType;
                    existeToken.DataExpiracao = dataExpiracao;
                    await _appDbContext.SaveChangesAsync();
                }
                else
                {
                    var novoToken = new DadosDropBox
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        TokenType = tokenType,
                        DataExpiracao = dataExpiracao
                    };
                    _appDbContext.DadosDropBox.Add(novoToken);
                    await _appDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar os tokens.");
            }
        }
    }


    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
    }
}
