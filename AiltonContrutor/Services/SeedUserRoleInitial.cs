using Microsoft.AspNetCore.Identity;

namespace AiltonContrutor.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                var role = new IdentityRole
                {
                    Name = "Member",
                    NormalizedName = "MEMBER"
                };
                await _roleManager.CreateAsync(role);
            }

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                await _roleManager.CreateAsync(role);
            }
        }

        public async Task SeedUsersAsync()
        {
            if (await _userManager.FindByEmailAsync("usuario@localhost") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "usuario@localhost",
                    Email = "usuario@localhost",
                    NormalizedUserName = "USUARIO@LOCALHOST",
                    NormalizedEmail = "USUARIO@LOCALHOST",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "Nusey#2024");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                }
            }

            if (await _userManager.FindByEmailAsync("felipesansi2012@gmail.com") == null)
            {
                var user = new IdentityUser
                {
                    UserName = "felipesansi2012@gmail.com",
                    Email = "felipesansi2012@gmail.com",
                    NormalizedUserName = "FELIPESANSI2012@GMAIL.COM",
                    NormalizedEmail = "FELIPESANSI2012@GMAIL.COM",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, "Nusey#2024");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
