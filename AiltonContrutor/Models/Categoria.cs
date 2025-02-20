using AiltonConstrutor.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaFacilEPS.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "O campo de Nome é obrigatório ")]
        [StringLength(100, ErrorMessage = "O tamanho maximo é de 100 caracteres")]
        [Display(Name = "Nome")]
        public required string CategoriaNome { get; set; }

        [Required(ErrorMessage = "O campo de Descrição é obrigatório ")]
        [StringLength(200, ErrorMessage = "O tamanho maximo é de 200 caracteres")]
        [Display(Name = "Descrição")]
        public required string Descricao { get; set; }

        public required List<Imovel> Imoveis { get; set; } = new List<Imovel>();
    }
}
