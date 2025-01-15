public class Utilizador
{
    public int Id_utilizador { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string TipoUtilizador { get; set; } 
    public string? Descricao_info { get; set; }= "Sem descrição.";
    public string? Servicos { get; set; } = "Serviços não especificados.";
}
