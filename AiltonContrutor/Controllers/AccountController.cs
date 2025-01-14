using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AiltonContrutor.ViewModels;
using System.Security.Claims;

namespace AiltonContrutor.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? retornoUrl = null)
        {
            var model = new LoginViewModel
            {
                UserName = string.Empty,
                Password = string.Empty,
                ReturnUrl = retornoUrl
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Adicionado para proteção contra CSRF
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Tenta autenticar o usuário
            var resultado = await _signInManager.PasswordSignInAsync(
                model.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                // Redireciona para a URL de retorno, se válida
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }

                // Redireciona para a página inicial caso nenhuma URL seja fornecida
                return RedirectToAction("Index", "Home");
            }

            // Caso a autenticação falhe, exibe uma mensagem de erro
            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Limpa a sessão
            HttpContext.User = null; // Limpa o usuário autenticado
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
