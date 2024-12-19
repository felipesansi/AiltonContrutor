using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiltonConstrutor.Models
{
    [Table("Imovel")]
    public class Imovel
    {
        [Key]
        public int IdImovel { get; set; }

        [Required(ErrorMessage = "O campo de titulo é obrigatório")]
        [Display(Name = "Titulo")]
        [StringLength(40, MinimumLength = 10, ErrorMessage = "O {0} deve ter o minimo {1} e o maximo {2} caracteres")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo de Descrição é obrigatório")]
        [Display(Name = "Titulo")]
        [StringLength(120, MinimumLength = 20, ErrorMessage = "O {0} deve ter o minimo {1} e o maximo {2} caracteres")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo de Quartos é obrigatório")]
        [Display(Name = "Números de quartos")]
        public required int Quartos { get; set; }

        [Required(ErrorMessage = "O campo de banheiros é obrigatório")]
        [Display(Name = "Números de banheiros")]
        public required int Banheiros { get; set; }

        [Display(Name = "Existe aréa Gurmet")]
        public bool AreaGurmet { get; set; }

        [Display(Name = "Existe churrasqueira")]
        public bool churrasqueira { get; set; }


        [Display(Name = "Existe piscina")]
        public bool piscina { get; set; }




    }
}
