namespace AiltonContrutor.Services
{
    public interface ISeedUserRoleInitial
    {
        Task SeedUsersAsync(); // Método assíncrono para inicializar usuários
        Task SeedRolesAsync(); // Método assíncrono para inicializar papéis
    }
}
