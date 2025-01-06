using AiltonConstrutor.Models;
using AiltonConstrutor.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using AiltonContrutor.Context;

namespace AiltonConstrutor.Repositorio
{
    public class ImovelRepositorio : IImovel
    {
        private readonly AppDbContext _appDbContext;

        public ImovelRepositorio(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Retorna todos os imóveis, incluindo suas categorias
        public IEnumerable<Imovel> imovels =>
            _appDbContext.Imoveis.Include(c => c.Categoria);

        // Retorna imóveis com o status "À venda"
        public IEnumerable<Imovel> ImovisPorStatus => _appDbContext.Imoveis.Include(c => c.Categoria).Where(p => p.StatusImovel == "À venda" || p.StatusImovel == "Em construção");

        // Retorna um imóvel pelo ID
        public Imovel? ImovelPorId(int idImovel) =>
            _appDbContext.Imoveis.Include(c => c.Categoria).FirstOrDefault(p => p.IdImovel == idImovel);

        // Implementação do membro da interface
        public IEnumerable<Imovel> ImoveisPorStatatus => _appDbContext.Imoveis.Include(c => c.Categoria).Where(p => p.StatusImovel == "À venda");
    }
}
