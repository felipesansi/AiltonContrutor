using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasaFacilEPS.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace CasaFacilEPS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly string _remetente;
        private readonly string _smtpServidor;
        private readonly string _senhaEmail;
        private readonly int _port;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

            DotNetEnv.Env.Load();
            _remetente = Environment.GetEnvironmentVariable("REMETENTE_EMAIL") ?? throw new InvalidOperationException("REMETENTE_EMAIL não encontrado.");
            _smtpServidor = Environment.GetEnvironmentVariable("SMTP_HOST") ?? throw new InvalidOperationException("SMTP_HOST não encontrado.");
            _senhaEmail = Environment.GetEnvironmentVariable("SENHA_EMAIL") ?? throw new InvalidOperationException("SENHA_EMAIL não encontrado.");
            _port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "587");
        }


        // Login (GET)
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

        // Login (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos");
            return View(model);
        }

        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public IActionResult RecuperarSenha()
        {
            return View();
        }

        
    
        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                TempData["Erro"] = "E-mail não informado.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["Erro"] = "E-mail não cadastrado no sistema.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetSenha", "Account", new { userId = user.Id, email = user.Email, token }, protocol: Request.Scheme);

            string corpo = $"Olá, identificamos uma tentativa de redefinição de senha no site Casa Fácil EPS.\n\n" +
                           $"Clique no link para redefinir sua senha: {callbackUrl}";

            try
            {
               
                using (SmtpClient client = new SmtpClient(_smtpServidor))
                {
                    client.Port = _port;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_remetente, _senhaEmail);
                    client.EnableSsl = true;
                    client.Timeout = 10000;

                    MailMessage emailMensagem = new MailMessage
                    {
                        From = new MailAddress(_remetente),
                        Subject = "Recuperação de Senha - Casa Fácil EPS",
                        Body = corpo,
                        IsBodyHtml = false
                    };

                    emailMensagem.To.Add(user.Email);
                    client.Send(emailMensagem);
                }

                TempData["Mensagem"] = "E-mail de recuperação enviado com sucesso! Verifique sua caixa de entrada.";
                return View();
            }
            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao enviar e-mail: {ex.Message}";
                return View();
            }
        }



        [HttpGet]
        public IActionResult ResetSenha(string userId, string token, string email)
        {
            var model = new ResetSenhaViewModel
            {
                UserId = userId,
                Token = token,
                Email = email
            };
            return View(model);
        }

       
        [HttpPost]
        public async Task<IActionResult> ResetSenha(ResetSenhaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null || string.IsNullOrEmpty(user.Email) || user.Email != model.Email)
            {
                TempData["Erro"] = "Usuário não encontrado.";
                return RedirectToAction("Index", "Home");
            }

            var resultado = await _userManager.ResetPasswordAsync(user, model.Token, model.NovaSenha);
            if (resultado.Succeeded)
            {
                TempData["Mensagem"] = "Senha redefinida com sucesso!";
                return RedirectToAction("Login", "Account");
            }

            foreach (var erro in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, erro.Description);
            }

            return View(model);
        }
    }
}
