using AiltonContrutor.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiltonConstrutor.Models
{
    [Table("Imovel")]
    public class Imovel
    {
        [Key]
        public int IdImovel { get; set; }

        [Required(ErrorMessage = "O campo de título é obrigatório")]
        [Display(Name = "Título")]
        [StringLength(40, MinimumLength = 10, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2} caracteres")]
        public required string Titulo { get; set; }

        [Required(ErrorMessage = "O campo de descrição é obrigatório")]
        [Display(Name = "Descrição")]
        [StringLength(120, MinimumLength = 20, ErrorMessage = "O {0} deve ter no mínimo {1} e no máximo {2} caracteres")]
        public required string Descricao { get; set; }

        [Required(ErrorMessage = "O campo de quartos é obrigatório")]
        [Display(Name = "Número de quartos")]
        public required int Quartos { get; set; }

        [Required(ErrorMessage = "O campo de banheiros é obrigatório")]
        [Display(Name = "Número de banheiros")]
        public required int Banheiros { get; set; }

        [Display(Name = "Existe área gourmet")]
        public bool AreaGourmet { get; set; }

        [Display(Name = "Existe churrasqueira")]
        public bool Churrasqueira { get; set; }

        [Display(Name = "Existe piscina")]
        public bool Piscina { get; set; }

        [Display(Name = "Valor do imóvel")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }


        public string? ImagemUrl { get; set; }

        public string? ImagemThumbnailUrl { get; set; }

        [Display(Name = "Status do Imóvel")]
        public string? StatusImovel { get; set; }

        public int CategoriaId { get; set; } // FK para Categoria

        [ForeignKey("CategoriaId")]
        public required Categoria Categoria { get; set; }
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
