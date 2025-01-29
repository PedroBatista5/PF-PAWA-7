
public class Projetos
{
    public int Id_projetos { get; set; }
    public string Titulo_projetos { get; set; }
    public double Preco { get; set; }
    public string Descricao_projeto { get; set; }
    public int Id_utilizador { get; set; }  // Esse é o ID do usuário que está criando o projeto
    
    public Utilizador Utilizador { get; set; }  // Relacionamento de navegação com Utilizadores
}

