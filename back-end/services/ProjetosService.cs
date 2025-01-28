using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SeuProjeto.Services;

namespace SeuProjeto.Services
{
    public class ProjetosService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProjetosService> _logger;

        public ProjetosService(AppDbContext context, ILogger<ProjetosService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Projetos> CriarProjeto(Projetos projeto)
        {
            try
            {
                if (projeto == null)
                {
                    throw new ArgumentNullException(nameof(projeto), "O projeto não pode ser nulo.");
                }

                if (string.IsNullOrWhiteSpace(projeto.Titulo_projetos) || 
                    string.IsNullOrWhiteSpace(projeto.Descricao_projeto) || 
                    projeto.Id_utilizador == 0)
                {
                    throw new ArgumentException("O título, descrição e ID do utilizador são obrigatórios.");
                }

                if (projeto.Preco <= 0)
                {
                    throw new ArgumentException("O preço do projeto deve ser maior que zero.");
                }

                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync(); // Salva as alterações no banco

                _logger.LogInformation("Projeto criado com sucesso");
                return projeto;
            }
            catch (DbUpdateException dbEx)
            {
                // Captura e exibe a inner exception (se disponível)
                var innerException = dbEx.InnerException != null ? dbEx.InnerException.Message : "Sem detalhes adicionais.";
                _logger.LogError(dbEx, "Erro ao salvar o projeto no banco de dados. Inner Exception: {InnerException}", innerException);
                throw new Exception($"Erro ao salvar o projeto: {innerException}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro geral ao criar o projeto.");
                throw new Exception("Erro ao salvar o projeto: " + ex.Message);
            }
        }

        public async Task<List<Projetos>> ObterProjetos()
        {
            try
            {
                var projetos = await _context.Projetos.ToListAsync();
                _logger.LogInformation("Projetos obtidos com sucesso. Total: {Count}", projetos.Count);
                return projetos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter os projetos.");
                throw new Exception("Erro ao obter projetos: " + ex.Message);
            }
        }

        public async Task<Projetos> ObterProjetoPorId(int id)
        {
            try
            {
                var projeto = await _context.Projetos.FirstOrDefaultAsync(p => p.Id_projetos == id);
                return projeto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar o projeto com ID {Id}", id);
                throw new Exception("Erro ao buscar projeto: " + ex.Message);
            }
        }
    }
}
