using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Utilizador
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(100)]
        public string Sobrenome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [StringLength(200)]
        public string Password { get; set; }
    }
}
