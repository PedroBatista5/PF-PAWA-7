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
                // Validações iniciais
                if (projeto == null)
                {
                    throw new ArgumentNullException(nameof(projeto), "O projeto não pode ser nulo.");
                }

                // Validar os campos obrigatórios
                if (string.IsNullOrWhiteSpace(projeto.Titulo_projetos) || 
                    string.IsNullOrWhiteSpace(projeto.Descricao_projeto) || 
                    projeto.Id_utilizador == 0)
                {
                    throw new ArgumentException("O título, descrição e ID do utilizador são obrigatórios.");
                }

                // Verificar se o utilizador existe
                var utilizador = await _context.Utilizadores.FindAsync(projeto.Id_utilizador);
                if (utilizador == null)
                {
                    throw new ArgumentException("O utilizador informado não existe.");
                }

                // Adicionar e salvar o projeto
                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Projeto criado com sucesso");
                return projeto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o projeto.");
                throw new Exception("Erro ao criar o projeto: " + ex.Message);
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

        public async Task<List<Contratacao>> ObterProjetosContratadosPorUsuario(int userId)
        {
            try
            {
                var contratacoes = await _context.Contratacoes
                                                .Where(c => c.Id_utilizador == userId)
                                                .Include(c => c.Projeto)
                                                .Include(c => c.Projeto.Utilizador)  // Inclui o criador do projeto
                                                .ToListAsync();
                return contratacoes;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter projetos contratados: " + ex.Message);
            }
        }


        public async Task<Projetos> ObterProjetoPorId(int id)
        {
            var projeto = await _context.Projetos
                .Include(p => p.Utilizador)
                .FirstOrDefaultAsync(p => p.Id_projetos == id);

            return projeto;
        }
    }
}
