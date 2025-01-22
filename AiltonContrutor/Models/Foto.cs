using AiltonConstrutor.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Foto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int ImovelId { get; set; }

    [ForeignKey("ImovelId")]
    public Imovel Imovel { get; set; }
}
