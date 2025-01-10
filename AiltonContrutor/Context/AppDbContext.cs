using Microsoft.EntityFrameworkCore;
using AiltonConstrutor.Models;
using AiltonContrutor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AiltonContrutor.Context
{
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Video> Videos { get; set; }
    }
}
