using AiltonConstrutor.Repositorio.Interfaces;
using AiltonContrutor.ViewModels;
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
            var vmImovel = new ImovelViewModel(); // Instancia a ViewModel
            vmImovel.Imoveis = _imovelRepositorio.imovels; // Obtém todos os imóveis
            vmImovel.CategoriaAtual = "Categoria Atual"; // Define a categoria atual

            return View(vmImovel); // Retorna a ViewModel para a view

        }
    }
}
