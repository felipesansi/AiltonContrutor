using AiltonConstrutor.Repositorio.Interfaces;
using CasaFacilEPS.Context;
using CasaFacilEPS.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasaFacilEPS.Controllers
{

    public class ImovelController : Controller
    {
      private readonly IImovel _imovelRepositorio;
      private readonly AppDbContext _context;

        public ImovelController(IImovel imovelRepositorio, AppDbContext context)
        {
            _imovelRepositorio = imovelRepositorio;
            _context = context;
        }
        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                var imovel = await _context.Imoveis
                    .Include(i => i.Fotos) // Carrega as fotos associadas ao imóvel
                    .Include(i => i.Videos) // Carrega os vídeos associadas ao imóvel
                    .FirstOrDefaultAsync(i => i.IdImovel == id);

                if (imovel == null)
                    return NotFound();

                var videosImovel = imovel.Videos; 

                return View(imovel);
            }
            catch (Exception ex)
            {
     
                return StatusCode(500, "Ocorreu um erro ao carregar os dados do imóvel."+ex);
            }
        }


    }
}
