using Microsoft.EntityFrameworkCore;
using AiltonConstrutor.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CasaFacilEPS.Models;

namespace CasaFacilEPS.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Imovel> Imoveis { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<DadosDropBox> DadosDropBox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Imovel>()
                .HasMany(imovel => imovel.Fotos)
                .WithOne(foto => foto.Imovel)
                .HasForeignKey(foto => foto.ImovelId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
