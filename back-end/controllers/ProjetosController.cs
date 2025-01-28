using Microsoft.AspNetCore.Mvc;
using SeuProjeto.Services;
using System.Linq;
using System.Security.Claims;

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
                // Obtém o ID do usuário do token
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id_utilizador");
                if (userIdClaim == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                // Atribui o ID do usuário logado ao projeto
                projeto.Id_utilizador = int.Parse(userIdClaim.Value);

                // Verificação para garantir que o Id_utilizador está sendo atribuído corretamente
                if (projeto.Id_utilizador == 0)
                {
                    return BadRequest("ID do usuário não encontrado.");
                }

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
                    return NotFound(new { mensagem = "Projeto não encontrado" });
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
