using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public interface IContratacaoService
{
    Task<Contratacao> CriarContratacaoAsync(int Id_utilizador, int Id_projetos, string statusContratacao);
}

public class ContratacaoService : IContratacaoService
{
    private readonly AppDbContext _context;

    public ContratacaoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Contratacao> CriarContratacaoAsync(int Id_utilizador, int Id_projetos, string statusContratacao)
    {
        // Aqui você pode adicionar qualquer lógica de validação ou outras verificações

        // Verificar se o cliente e o projeto existem
        var cliente = await _context.Utilizadores.FindAsync(Id_utilizador);
        var projeto = await _context.Projetos.FindAsync(Id_projetos);

        if (cliente == null)
        {
            throw new ArgumentException("Cliente não encontrado.");
        }

        if (projeto == null)
        {
            throw new ArgumentException("Projeto não encontrado.");
        }

        var contratacao = new Contratacao
        {
            Id_utilizador = Id_utilizador,
            Id_projetos = Id_projetos,
            Status_contratacao = statusContratacao,
            Data_contratacao = DateTime.Now
        };

        _context.Contratacoes.Add(contratacao);
        await _context.SaveChangesAsync();

        return contratacao;
    }
}