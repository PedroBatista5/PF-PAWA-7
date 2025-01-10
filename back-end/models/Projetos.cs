using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Projetos
    {
        public int Id_projetos { get; set; }

        [Required]
        [StringLength(100)]
        public string titulo_projetos { get; set; }

        [Required]
        [StringLength(100)]
        public double preco { get; set; }

        [Required]
        [StringLength(200)]
        public string descricao_projeto { get; set; }

        public byte[] imagem { get; set; }

        public ICollection<InfoUser> InfoUsers { get; set; }

    }
}
