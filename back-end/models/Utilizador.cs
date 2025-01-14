public class Utilizador
{
    public int Id_utilizador { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string TipoUtilizador { get; set; } // "cliente" ou "freelancer"

    // Campos de InfoUser integrados
    public string Descricao_info { get; set; }
    public string Servicos { get; set; }
    public string Imagem_perfil { get; set; }
}
