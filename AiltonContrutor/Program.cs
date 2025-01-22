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
builder.Services.AddScoped<IUploadFotosService>(provider =>
    new UploadFotosService("sl.u.AFedJkJN07ItY1PhiqPivFaDhF0xyIRuKy3b2ww_M_HkdOOXJ5jn_KWswdkK1U8ugJFF67roZaj3cvldQ_Pklff4K7nRxb9KiI0wGon6wwjQ5Kk9pV3fT4krkCFdPx9gAXJV9KiYrnRsAb_PlbkJ5rxB5nxueVAem7Xuk-1RDNaHYrImZkD9_gyOBXFsk1vJVytxqKTbD_m4bYGIqN88n2wACRxRFYRpCTEaP_-08TVdfwGOGPNlvy7hlRgJmFmq-j4Mtt9iJhMAYI6mGCD9mmVhI5EEwn4BK54CZaXIwyjlhD_eK7rzkyYb1p8k3P6LHqxLwh2BD6V5Z7_crUF3NilNZ-y6kCeD2D89sRvLcMzcQthvr9aYvchKi9Yqc8P05kitDZ8A21xg5Si8i5RUPCh4600npkke1bV6M6UNVGDTpXpU2GkEaRaDZOans9jm3q88-cdQzPtMEYTdLyJUwd3fFaQkl5eamYC0_AYVRUOK86zybRKjN-gqySry8NRfvIsleH8rLQaNd4JJoh_BR4TwiQZzgCVCOuipSp0Tqxv8H_o2x0FiyxTkIGvKWeo-abOqA9ba9Z98avkRJpidWX3Q0e90lNWqF8NO7cJ0R6teMN6_lNQxoa3tAI9V3bESstiHp9E2ChBZ2wCC6siGTkRX0oKBSLPEDiJQCyktGk1eycMuCfaC4rhoTFj6sO40zCWsjMwAjc43GtprfZNumlgWa7dXrYcaeW-_-l-hphPb-vVj0Jpok7cBOKXadgQr1oiQVLaajJTYB0TGKJuw6ff-E3E5bSbW7OuS9RN3Mr9yZ7P3r4F3KxGb6tYYNXAE0-7GoWJwF91x6JXpgd9vNDiN29chNAG6ySmr3dO4AYvtVGZSUcIPVT3axhUcK_aujsVJoQ5AyvSw3K-q3awJUrcdn9msmSOsl5U2-YfzNNqZci2jaVU_t8pxLar_jLKH4GaWmW3vVfXdn1HQqsnkrtIlsh2nqVUjbI6UoN_Rko6pn2TC4QnCmaWhTfrEBfYGHUX7iJFIwMbHM3I7Ol1N4WUJ8-wg8efU42m57-6opgHJDcvAaFk6xXT8S73mfskFr9gD2z5SyBuGSOpHzzujT36WofhBDCLFmzgJpgByl0_yBrsyIoTwQGhfVkDx5kAfSAXEvBG-KEpFaj8uIf7-HUllfXwGiZakaejerc4_AH1rIV1Kox7BY8JpLSFeIrCMRuA3cpMx968l4kv-8A4sg6_po_9_OeN2O57teElOlAUypd5R8-9kA7ZB3S8HlPirFzt5BTtltE00osZ3qL1-HDUMsCAYei4WSjyuens_Xv-aejsj_2Q440lk6gfkygKNBISPbJN2TemQrDpfi-2HbqweeV8gEZNEV0Okxa0HgegX4ufA4VWtXW7hBWCPg-UY_nLIebzFSOllqLjOC5KkqZ0aGBzwFo5jFbfCOlk-Mdd9civOeQZWL-kk1829Et_BJVM"));

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
