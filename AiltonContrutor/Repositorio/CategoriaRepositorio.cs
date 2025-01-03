using AiltonContrutor.Context;
using AiltonContrutor.Models;
using AiltonContrutor.Repositorio.Interfaces;

namespace AiltonContrutor.Repositorio
{
    public class CategoriaRepositorio : ICategoria
    {
        private readonly AppDbContext _appDbContext;
        public CategoriaRepositorio(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Categoria> Categorias => _appDbContext.Categorias;
    }
}
