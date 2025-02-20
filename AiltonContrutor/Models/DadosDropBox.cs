﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasaFacilEPS.Models
{
    [Table("DadosDropBox")]
    public class DadosDropBox
    {
        [Key]
        public int Id { get; set; }
        public string AccessToken { get; set; }
        [MaxLength(100)]
        public string RefreshToken { get; set; }
        [MaxLength(100)]
        public string TokenType { get; set; }
        public DateTime DataExpiracao { get; set; }

        public DadosDropBox()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            TokenType = string.Empty;
        }
    }
}
