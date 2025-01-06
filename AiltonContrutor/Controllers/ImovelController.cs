using AiltonConstrutor.Repositorio.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiltonContrutor.Controllers
{
    public class ImovelController : Controller
    {
      private readonly IImovel _imovelRepositorio;

        public ImovelController(IImovel imovelRepositorio)
        {
            _imovelRepositorio = imovelRepositorio;
        }
        public IActionResult List()
        {
            var imoveis = _imovelRepositorio.imovels;
            return View(imoveis);

        }
    }
}
