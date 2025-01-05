namespace Backend.Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Sobrenome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }

}
