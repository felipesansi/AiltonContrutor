namespace AiltonContrutor.Repositorio.Interfaces
{
    public interface IUploadFotosService
    {
        Task<string> UploadFileAsync(IFormFile file, string caminho_destino);
    }
}
