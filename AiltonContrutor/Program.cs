using AiltonConstrutor.Repositorio.Interfaces;
using AiltonConstrutor.Repositorio;
using AiltonContrutor.Context;
using AiltonContrutor.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AiltonContrutor.Repositorio.Interfaces;
using AiltonContrutor.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configura��o do banco de dados (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configura a pol�tica de autoriza��o
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", politica =>
    {
        politica.RequireRole("Admin");
    });
});

// Configura��o do reposit�rio
builder.Services.AddTransient<IImovel, ImovelRepositorio>();
builder.Services.AddTransient<ICategoria, CategoriaRepositorio>();
builder.Services.AddTransient<IVideo, VideoRepositorio>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

// Configura��o do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configura��o do servi�o de sess�o
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Expira��o da sess�o
    options.Cookie.HttpOnly = true;                // Restringe acesso ao cookie apenas via HTTP
    options.Cookie.IsEssential = true;             // Necess�rio para a funcionalidade da aplica��o
    options.Cookie.Name = ".AiltonConstrutor.Session"; // Nome do cookie de sess�o
});


var app = builder.Build();

// Configure o pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Redireciona para p�gina de erro
    app.UseHsts(); // Configura o cabe�alho HSTS
}

app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
app.UseStaticFiles();      // Habilita arquivos est�ticos (CSS, JS, imagens)

app.UseRouting();          // Configura o roteamento

app.UseSession();          // Habilita middleware de sess�o
app.UseAuthentication();   // Configura a autentica��o
app.UseAuthorization();    // Configura a autoriza��o

// Configura��o de rotas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    try
    {
        var services = scope.ServiceProvider;

        // Aplica migra��es pendentes
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        // Inicializa pap�is e usu�rios
        var seedUserRolesInitial = services.GetRequiredService<ISeedUserRoleInitial>();
        await seedUserRolesInitial.SeedRolesAsync();
        await seedUserRolesInitial.SeedUsersAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar o banco de dados: {ex.Message}");
    }
}

app.Run();
