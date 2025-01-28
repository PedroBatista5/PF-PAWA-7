
public class Contratacao
{
    public int Id_contratacao { get; set; }

    public int Id_utilizador { get; set; }  

    public int Id_projetos { get; set; }  

    public DateTime Data_contratacao { get; set; }
    
    public string Status_contratacao { get; set; }

    public Utilizador Cliente { get; set; }

    public Projetos Projeto { get; set; }
}
