using Microsoft.AspNetCore.Mvc;
using SeuProjeto.Services;
namespace SeuProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetosService _projetosService;

        public ProjetoController(ProjetosService projetosService)
        {
            _projetosService = projetosService;
        }

        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarProjeto([FromBody] Projetos projeto)
        {
            try
            {
                var novoProjeto = await _projetosService.CriarProjeto(projeto);
                return Ok(novoProjeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar projeto: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("todos")]
        public async Task<IActionResult> ObterProjetos()
        {
            try
            {
                var projetos = await _projetosService.ObterProjetos();
                return Ok(projetos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter projetos: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjetoById(int id)
        {
            try
            {
                var projeto = await _projetosService.ObterProjetoPorId(id);
                if (projeto == null)
                {
                    return NotFound(new { mensagem = "Projeto não encontrado" }); // Mensagem customizada
                }
                return Ok(projeto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar o projeto: {ex.Message}");
            }
        }



    }
}
