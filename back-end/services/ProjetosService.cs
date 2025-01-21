using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Projeto criado com sucesso: {@Projeto}", projeto);
                return projeto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o projeto.");
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
    }
}
