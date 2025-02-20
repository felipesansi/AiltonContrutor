using System.Threading.Tasks;
using CasaFacilEPS.Models;
using Microsoft.AspNetCore.Http;

namespace CasaFacilEPS.Repositorio.Interfaces
{
    public interface IUploadFotosService
    {
        
        Task<string> UploadFileAsync(IFormFile file, string caminho_destino);

       
        Task SalvarTokensAsync(string accessToken, string refreshToken, string tokenType, DateTime dataExpiracao);

       
        Task<DadosDropBox> RecuperarTokenActivoAsync();
    }
}
