using Microsoft.EntityFrameworkCore;
using AiltonConstrutor.Models;
using AiltonContrutor.Models;

namespace AiltonContrutor.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
