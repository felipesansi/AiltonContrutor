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

// Configuração do banco de dados (SQL Server)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configura a política de autorização
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", politica =>
    {
        politica.RequireRole("Admin");
    });
});

// Configuração do repositório
builder.Services.AddTransient<IImovel, ImovelRepositorio>();
builder.Services.AddTransient<ICategoria, CategoriaRepositorio>();
builder.Services.AddTransient<IVideo, VideoRepositorio>();
builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

// Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuração do serviço de sessão
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Expiração da sessão
    options.Cookie.HttpOnly = true;                // Restringe acesso ao cookie apenas via HTTP
    options.Cookie.IsEssential = true;             // Necessário para a funcionalidade da aplicação
    options.Cookie.Name = ".AiltonConstrutor.Session"; // Nome do cookie de sessão
});


var app = builder.Build();

// Configure o pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Redireciona para página de erro
    app.UseHsts(); // Configura o cabeçalho HSTS
}

app.UseHttpsRedirection(); // Redireciona HTTP para HTTPS
app.UseStaticFiles();      // Habilita arquivos estáticos (CSS, JS, imagens)

app.UseRouting();          // Configura o roteamento

app.UseSession();          // Habilita middleware de sessão
app.UseAuthentication();   // Configura a autenticação
app.UseAuthorization();    // Configura a autorização

// Configuração de rotas
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

        // Aplica migrações pendentes
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        // Inicializa papéis e usuários
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
