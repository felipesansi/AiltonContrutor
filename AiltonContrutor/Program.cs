using AiltonConstrutor.Repositorio;
using AiltonConstrutor.Repositorio.Interfaces;
using AiltonContrutor.Context;
using AiltonContrutor.Repositorio;
using AiltonContrutor.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuração do banco de dados(SQL Server): AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configuração do banco de dados(SQL Server): AppDbContext

// Configuração do Repositório
builder.Services.AddScoped<IImovel, ImovelRepositorio>();
builder.Services.AddScoped<ICategoria, CategoriaRepositorio>();
builder.Services.AddScoped<IVideo, VideoRepositorio>();
// Configuração do Repositório

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
           name: "areas",
           pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
         );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
