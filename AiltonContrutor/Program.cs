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
new UploadFotosService("sl.u.AFfGrIiXRFClY-NOcUgq6t4M0Up378N4XlRjAadeSOmliumH3J4GWh2V7XMhT0aNuqqH2SDfy7zZyHCSiCJubu-9Mgez89flzeAwizt7rAoMjSCsa8jCw-gc12oX_yIGz9ZT0tSetKFSumuyhocRT_GPymvDBdIuDcf3xkSxuI52hFEfealGqFbPpw3PzwzIlwt19nhuJM9UauQo18MPNLp-RyDne8z-RgN2ZbVzhM8SjsU-m37Wfd42uaofBNH4nwgVOFgGiQ9oE8DCM45ZTEx7BlRYJwgBsMeToqMFX4K4ILrUVHs4-fHdlGGRV2MU9qSfBLwapz4aBuqWi-a27fJ4EBv1UttiVdleD7C44xn1IgLqp7L_n0ldfoul2EF8vYkSz4jIvVHaXoIJAhJuBBzRFasRu_AZicLs0vz2vWiLRJHx1TeUurprm9UGNz9jztBdOCvgSK9QVLHyzofjXcU2i3GOmekJIwNuB2hka4a5iUn9sh3dZM_VDK6dxrTzsliQSBVOEVUSpIYuXCgqwnmAwyY3cFn2di281AcF8wCRa_aJATblzvLA-o_5Lisy9w0GPz-g3dUS2r4jkxjqxH_PxB3EDr-Z1RQHKToYjJCMZDh8Ua6CKEOZuqWA-KnVyQRw7fa2fZuvSuk0W1mFXwjVYIIl6RMba0La7htVjNbkYGQP9ORcPbjpcoySNzSqDR74sxQaoVxZVhYRN3owdN7mxOp6QjG1uYnMRj7BP2n4LX3aTmQb0-po6LUXBqCicKeoCMODcEzdUTf-odZ2QkZBJZzzkZeNpu3ZaidP2yIffDrZg64rY1EAY7UpnJ4kCYY2XAQcelFQQ2vB0bHibpI2Q_6XJWbrzOb4ksrLQ83i7rwuPT6mfJg_EMBCK-L1Wpyub5AyOWWdo-fBS2Q51RwH9N-_XWo5WVh1NYFfysqKNqz7pnhZv1G5nbKb4n_nhkJas-IYyRN3mjJnxxnu5SnT3HSIYczPg-0Tg_EA_6LM-vmOTaKEg9WbEYI4jzJq4F3crnnO2PlMdqIcpkJp7f-RN3zKteFg-ouxkkJz6g41TKS_rV1PEEjqNNTHxtrlSCVe87Z1O282vI5TNRw2FLBRzHeBgNrg851odEnFwdXX6Q64MvjRLIFggoeBXRNKLsxQrG_2KRbtgsK-STdVXolYwleh9mTsw9K-Mbjwo3Zbiq87_-RcGbjuqMIBd6HG45o89qyKop1zMUOzL-oo6sx4-HDQ4i1sNZFQvlyDIf5Vz7olTDggFJAl28Y6G5eWysnoQXethtgrdSNKHyY3kZHNikDocA3q0XemMUi0VYl2hOX7w8kMaonGx8UIokY7o1zf71Vxbj5t5bmCuY6yZsefMym8fTErCUY0W3nufvCQjU0vvhxLLf6_ZUyxq7SP9HenI7q8rxSY1ZTjOW7CZVzhCD6Yqyp6RQJdZ2_rkSeMTEVVaMKffCRj0hIgVF-LdDA"));


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
