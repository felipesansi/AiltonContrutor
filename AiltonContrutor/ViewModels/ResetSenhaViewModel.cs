using System.ComponentModel.DataAnnotations;

namespace CasaFacilEPS.ViewModels
{
    public class ResetSenhaViewModel
    {
      
        [EmailAddress(ErrorMessage = "O e-mail fornecido não é válido.")]
        public string Email { get; set; } = string.Empty; // Valor padrão para evitar nulos

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [StringLength(100, ErrorMessage = "A senha deve ter pelo menos {2} caracteres.", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmarSenha { get; set; } 

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}