using AiltonConstrutor.Models;

namespace AiltonContrutor.ViewModels
{
    public class ImovelViewModel
    {
        public IEnumerable<Imovel>? Imoveis { get; set; }
        public string ?CategoriaAtual { get; set; }
    }
}
