using AiltonContrutor.Repositorio.Interfaces;
using Dropbox.Api;
using Dropbox.Api.Files;
using System.IO;

namespace AiltonContrutor.Repositorio
{
    public class UploadFotosService : IUploadFotosService
    {
        private readonly string _dropBoxToken;

        public UploadFotosService(string dropBoxToken)
        {
            _dropBoxToken = dropBoxToken;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string caminho_destino)
        {
           
                    using (var dbx = new DropboxClient(_dropBoxToken))
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
    } 
}
