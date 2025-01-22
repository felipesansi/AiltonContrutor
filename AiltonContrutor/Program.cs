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
    new UploadFotosService("sl.u.AFeXhGrOQzdzLK4qsGDujiiin38tKFWdirzBMvjTMu3ntr8Kx6CIvqI1i0AECMQKJcUFxDHgXmzTYC2hf9Q5IhAzMZQ1cgWelS6oMuIdWF8v4ObBPQ1w3SDWMDYj9Kxs1mN4m8MVoSCoqUicijjqTDqXQlZ_TXTwwiQotqd3T34aymW2NWDyaaOIHuLErD1RXXMAFZizgbow83kH2-tgiRq0hyXAGftPuA8bsHEWHvM7_WBQK82wmcv9NOCL09cux9PZBNWzqAo6GisfTYXcHzy1q9q5qZ6wgpLQh2zHxzJmh7kmDDoO7pbQ50j6-q5B9mZECaYSzcwkGyakW8u4jry5rzfQd-q4Pd68-1QTfTnNds42L1gu3fvOJI46qUribKjRSocbaJPysL3aPTGRu86nAbuD_BjX_g5N-_3FHYOsrNkOuHAnlm5qTHrLIMwGtPSy7N1Dz7soLwPtExTnxJQsDZm5sgReeeJZtUDJX3O3Bq_k1ggN6b39Rka4qnoPKmqEUXkUAwOZZlKgeHmQr_evyvryJTfrtJjtn3GAnFHWPxh5wfEyRhfkvPFq2BrFoPuBD4zD13NAhuiscRVwQUvb2hbJIzlAvkVPO9DyR9UtI3FnFLWmvtj1sxaD0ddhNf4QD8caFdVTzLMQ30bXeirRfgL_4RyftJqLwkziig_rnjoli5I5TB-moGGJ53jP9bfEteztWE6sIh46YY-2SYUL6X3uktvQbPPwsSIVx_koyLUOAS_pcg7R3-dR4f_85M_jSTnEnK_brAtUk6HtWkyrEeDbrzj9InXIdEPpRbLQXAJYGfrj0mW-jDQYWUrj1mG30gt7AgscQ6GjjZNehda3sSKMXI6-jI3Mobz1CNMrdzOlSFD2T1MYixqratRPYmpfxa0uWtjxpgCQqwkgI2LdAIVKP-wl0BGhs6btznLu2ySwEiI83hdQTLV8HsRuXKQz6tICundL_jHJ0KlDlTEit2eyXEzZjxxGWcY1v9cMEBcO7dCicjPe0qajJenixxJgiB1m0PYcTGfmpCXbLNEs6oKERB2cVHtFMZTMP96CTWXVIOTtTMIrO2xdyM9UTOtNnuNwACHCmGByIJIk-GzPSogt0kBFJY3HfInnC6KSqi9pXDDEQKbwbDekltIXUJq4yt14glWGg5NEGJRztUxYLTz-N3XXl3c9gxHWxJUEOzvYOFyAHi8zR-FAb3yH3odNF3G9_8UIt7LU9miVV2SCf6PzYa_JatPLA0UTX_EjA7G4t-HmPYo7WUps-M4L3Bl6kI91Gz1IWpTOrN0atjzCpuMpjxAdIsZMKh3Qy-k_vff4SPMuc76gWb5fbutIl_ph-UlYn0WvDNEyfRzlHDaxUeHXW7I_xuLsIG_NaQA-9zJ8rq587t3JGScjHeYmbLoNdkHW5R9Lf-DVwILjY6w8XqhaHSWj5qXCawG4HDt4zLIjALATlbh3GpmVCswCCTw"));

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
