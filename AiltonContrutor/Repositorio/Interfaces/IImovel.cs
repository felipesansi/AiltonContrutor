using AiltonConstrutor.Models;

namespace AiltonContrutor.Repositorio.Interfaces
{
    public interface IImovel
    {
        IEnumerable<Imovel> Imovel { get; }
        IEnumerable<Imovel> ImovelAvenda { get; }
        Imovel GetImovelById(int idImovel);
    }
}
