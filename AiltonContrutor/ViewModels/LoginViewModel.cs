using System.ComponentModel.DataAnnotations;

namespace AiltonContrutor.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o nome de usuário")]
        [Display(Name = "Usuário")]
        public string UserName { get; set; } = string.Empty; // Valor padrão para evitar nulos

        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty; // Valor padrão para evitar nulos

        public string? ReturnUrl { get; set; }
    }
}
