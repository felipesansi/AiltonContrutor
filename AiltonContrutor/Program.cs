using AiltonConstrutor.Repositorio.Interfaces;
using AiltonConstrutor.Repositorio;
using CasaFacilEPS.Context;
using CasaFacilEPS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CasaFacilEPS.Repositorio.Interfaces;
using CasaFacilEPS.Repositorio;

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
builder.Services.AddScoped<IUploadFotosService, UploadFotosService>();

// Configuração do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuração da senha
    options.Password.RequireDigit = false; // Não exige número
    options.Password.RequiredLength = 6; // Mínimo de 6 caracteres
    options.Password.RequireNonAlphanumeric = false; // Não exige caractere especial
    options.Password.RequireUppercase = false; // Não exige letra maiúscula
    options.Password.RequireLowercase = false; // Não exige letra minúscula
    options.Password.RequiredUniqueChars = 1; // Mínimo de caracteres únicos
});

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
    name: "login",
    pattern: "Login",
    defaults: new { controller = "Account", action = "Login" });

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
