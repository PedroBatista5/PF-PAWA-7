using Microsoft.EntityFrameworkCore;

namespace SeuProjeto.Services
{
    public class ProjetosService
    {
        private readonly AppDbContext _context;

        public ProjetosService(AppDbContext context)
        {
            _context = context;
        }

        // Criar um novo projeto
        public async Task<Projetos> CriarProjeto(Projetos projeto)
        {
            try
            {
                _context.Projetos.Add(projeto);
                await _context.SaveChangesAsync();
                return projeto;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar o projeto: " + ex.Message);
            }
        }

        public async Task<List<Projetos>> ObterProjetos()
        {
            try
            {
                return await _context.Projetos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter projetos: " + ex.Message);
            }
        }
    }
}
