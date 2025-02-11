using System.Diagnostics;
using AiltonConstrutor.Repositorio.Interfaces;
using AiltonContrutor.Context;
using AiltonContrutor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiltonContrutor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private readonly IImovel _imovelRepositorio;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IImovel imovelRepositorio)
        {
            _logger = logger;
            _context = context;
            _imovelRepositorio = imovelRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            var imoveis = await _context.Imoveis.ToListAsync();
            return View(imoveis); // Envia a lista de imóveis para a view principal
        }

        public IActionResult Contato()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public ActionResult Agradecimento()
        {
            return View();
        }
        public ActionResult Eps()
        {
            return View();
        }

    }
}
