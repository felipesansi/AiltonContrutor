using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiltonConstrutor.Models
{
    [Table("Foto")]
    public class Foto
    {
        [Key]
        public int FotoId { get; set; }

        [Required]
        [StringLength(200)] // Tamanho máximo da URL
        public string Url { get; set; }

        [Required]
        public int ImovelId { get; set; } // FK para Imovel

        [ForeignKey("ImovelId")]
        public Imovel Imovel { get; set; }
    }
}
