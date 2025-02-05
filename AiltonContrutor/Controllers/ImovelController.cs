using AiltonConstrutor.Repositorio.Interfaces;
using AiltonContrutor.Context;
using AiltonContrutor.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiltonContrutor.Controllers
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
            var imovel = await _context.Imoveis
                .Include(i => i.Fotos) // Carrega as fotos associadas ao imóvel
                .FirstOrDefaultAsync(i => i.IdImovel == id);

            if (imovel == null)
                return NotFound();

            return View(imovel);
        }

    }
}
