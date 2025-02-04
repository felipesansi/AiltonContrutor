using AiltonConstrutor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AiltonContrutor.Context;
using AiltonContrutor.Repositorio.Interfaces;

namespace AiltonConstrutor.Areas.Admin.Controllers
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
            _context = context;
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

            var fotosImovel = await _context.Fotos.Where(f => f.ImovelId == imovelId).ToListAsync();

            if (!fotosImovel.Any())
            {
                string nomeFoto = Path.GetFileName(foto.FileName);
                if (!nomeFoto.Contains("Capa", StringComparison.OrdinalIgnoreCase))
                {
                    ViewData["MensagemErro"] = "Nome da imagem NÃO contém 'Capa'!";
                    return View();
                }
            }

            var nomeUnicoArquivo = $"{Guid.NewGuid()}_{foto.FileName}";
            var caminhoDestino = $"/Imagens/{nomeUnicoArquivo}";

            try
            {
                var linkGerado = await _uploadFotosService.UploadFileAsync(foto, caminhoDestino);
                var linkAjustado = linkGerado.Replace("dl=0", "raw=1");

                var imovel = await _context.Imoveis.FindAsync(imovelId);
                if (imovel == null)
                {
                    ViewData["MensagemErro"] = "Imóvel não encontrado.";
                    return View();
                }

                var novaFoto = new Foto
                {
                    Url = linkAjustado,
                    ImovelId = imovelId,
                };

                _context.Fotos.Add(novaFoto);
                await _context.SaveChangesAsync();

                string nomeFoto = Path.GetFileName(foto.FileName);
                if (nomeFoto.Contains("Capa", StringComparison.OrdinalIgnoreCase))
                {
                    imovel.ImagemUrl = linkAjustado;
                    _context.Imoveis.Update(imovel);
                    await _context.SaveChangesAsync();
                }
                // // Recupera todas as fotos do imóvel para o carrossel
                // var fotosCarrossel = await _context.Fotos
                //     .Where(f => f.ImovelId == imovelId)
                //     .ToListAsync();
                // //return PartialView("_CarrosselFotos", fotosCarrossel);
                //// return View();


                ViewData["LinkImagem"] = linkAjustado;
                ViewData["MensagemSucesso"] = "Imagem enviada e salva com sucesso!";
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
