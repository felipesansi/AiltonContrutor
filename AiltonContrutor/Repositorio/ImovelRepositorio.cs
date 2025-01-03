using AiltonConstrutor.Models;
using AiltonContrutor.Context;
using AiltonContrutor.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AiltonContrutor.Repositorio
{
    public class ImovelRepositorio : IImovel
    {
        private readonly AppDbContext _appDbContext;

        public ImovelRepositorio(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Imovel> Imovel => _appDbContext.Imoveis.Include(c => c.Categoria);

        public IEnumerable<Imovel> ImovelAvenda => _appDbContext.Imoveis.Include(c => c.Categoria).Where(p => p.ImovelAVenda);

        public Imovel? GetImovelById(int idImovel)
        {
            return _appDbContext.Imoveis.FirstOrDefault(p => p.IdImovel == idImovel);
        }

    }
}
