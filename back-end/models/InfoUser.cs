using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class InfoUser
    {
        public int Id_info { get; set; }

        [StringLength(500)]
        public string Descricao_info { get; set; }

        [StringLength(200)]
        public string Servicos { get; set; }

        public ICollection<Utilizador> Utilizadores { get; set; }

        public int? ProjetosId { get; set; } // Tornar anulável

        public Projetos Projetos { get; set; }
    }
}
