using AiltonConstrutor.Models;

namespace AiltonConstrutor.Repositorio.Interfaces
{
    public interface IImovel
    {
        IEnumerable<Imovel> imovels { get; }
        IEnumerable<Imovel> ImoveisPorStatatus { get; } // Obtém imóveis por status
        Imovel? ImovelPorId(int idImovel); // Obtém imóvel por ID
    }
}
