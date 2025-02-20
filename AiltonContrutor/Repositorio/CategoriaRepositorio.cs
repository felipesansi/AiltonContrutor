using CasaFacilEPS.Context;
using CasaFacilEPS.Models;
using CasaFacilEPS.Repositorio.Interfaces;

namespace CasaFacilEPS.Repositorio
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
