using AiltonConstrutor.Models;
using AiltonContrutor.Repositorio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AiltonContrutor.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminFotoController : Controller
    {
        private readonly IUploadFotosService _uploadFotosService;

        public AdminFotoController(IUploadFotosService uploadFotosService)
        {
            _uploadFotosService = uploadFotosService;
        }

        [HttpGet]
        public IActionResult EnviarFoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarFoto(IFormFile foto)
        {
            if (foto == null || foto.Length == 0)
            {
                ViewData["MensagemErro"] = "Por favor, selecione uma imagem válida.";
                return View();
            }

            // Define o caminho no Dropbox
            var caminho_destino = $"/Imagens/{foto.FileName}";
            try
            {
              
                var link_gerado = await _uploadFotosService.UploadFileAsync(foto, caminho_destino);

             
                ViewData["MensagemSucesso"] = "Imagem enviada com sucesso!";
                ViewData["LinkImagem"] = link_gerado;

                return View();
            }
            catch (Exception ex)
            {
                ViewData["MensagemErro"] = $"Erro ao enviar a imagem: {ex.Message}";
                return View();
            }
        }
    }
}
