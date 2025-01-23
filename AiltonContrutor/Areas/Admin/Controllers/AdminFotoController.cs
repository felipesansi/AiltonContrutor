using AiltonConstrutor.Models;
using AiltonContrutor.Context;
using AiltonContrutor.Repositorio.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Gera um nome único para o arquivo
            var nomeUnicoArquivo = $"{Guid.NewGuid()}_{foto.FileName}";
            var caminho_destino = $"/Imagens/{nomeUnicoArquivo}";

            try
            {
                // Faz o upload da imagem para o Dropbox e obtém o link gerado
                var link_gerado = await _uploadFotosService.UploadFileAsync(foto, caminho_destino);
                var link_ajustado = link_gerado.Replace("dl=0", "raw=1");
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
                    Url = link_ajustado,
                    ImovelId = imovelId,
                };

                _context.Fotos.Add(novaFoto);
                await _context.SaveChangesAsync();

                // Recupera todas as fotos do imóvel para o carrossel
                var fotosImovel = await _context.Fotos
                    .Where(f => f.ImovelId == imovelId)
                    .ToListAsync();

                // Renderiza a partial view com as fotos
                ViewData["MensagemSucesso"] = "Imagem enviada e salva com sucesso!";
                return PartialView("_CarrosselFotos", fotosImovel);
            }
            catch (Exception ex)
            {
                ViewData["MensagemErro"] = $"Erro ao enviar a imagem: {ex.Message}";
                return View();
            }
        }

    }
}
