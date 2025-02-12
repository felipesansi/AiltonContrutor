using AiltonConstrutor.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace AiltonConstrutor.Models
{
    public class Video
    {
        [Key]
        public int IdVideo { get; set; }

        [ForeignKey("Imovel")]
        public int IdImovel { get; set; }

        // Propriedade de navegação opcional para associar ao imóvel
        public Imovel? Imovel { get; set; }

        [Required]
        [Url(ErrorMessage = "A URL deve ser válida")]
        public required string Url { get; set; }
    }
}