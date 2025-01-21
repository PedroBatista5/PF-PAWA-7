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

        // Criar um novo projeto
        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarProjeto([FromForm] Projetos projeto)
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


    }
}
