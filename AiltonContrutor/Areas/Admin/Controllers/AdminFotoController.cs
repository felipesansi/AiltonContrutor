using AiltonConstrutor.Models;
using AiltonContrutor.Context;
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
        private readonly AppDbContext _context;

        public AdminFotoController(IUploadFotosService uploadFotosService, AppDbContext context)
        {
            _uploadFotosService = uploadFotosService;
            _context = context; // Injeta o contexto do banco de dados
        }

        [HttpGet]
        public IActionResult EnviarFoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EnviarFoto(int imovelId, IFormFile foto)
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
                // Faz o upload da imagem para o Dropbox e obtém o link gerado
                var link_gerado = await _uploadFotosService.UploadFileAsync(foto, caminho_destino);

                // Recupera o imóvel correspondente no banco de dados
                var imovel = await _context.Imoveis.FindAsync(imovelId);

                if (imovel == null)
                {
                    ViewData["MensagemErro"] = "Imóvel não encontrado.";
                    return View();
                }

                // Cria uma nova entrada para a tabela de fotos
                var novaFoto = new Foto
                {
                    Url = link_gerado,
                    ImovelId = imovelId,
                };

                _context.Fotos.Add(novaFoto);
                await _context.SaveChangesAsync();

                ViewData["MensagemSucesso"] = "Imagem enviada e salva com sucesso!";
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
