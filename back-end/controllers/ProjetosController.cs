using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;


        public ProjetoController(ProjetosService projetosService, AppDbContext context)
        {
            _projetosService = projetosService;
            _context = context;
        }

        [HttpPost]
        [Route("criar")]
        public async Task<IActionResult> CriarProjeto([FromBody] Projetos projeto)
        {
            try
            {
                Console.WriteLine("Dados recebidos do Frontend: " + System.Text.Json.JsonSerializer.Serialize(projeto));

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }
                int userId = int.Parse(userIdClaim.Value);
                projeto.Id_utilizador = userId;

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

                if (projeto.Utilizador == null)
                {
                    return NotFound(new { mensagem = "Usuário não encontrado para o projeto" });
                }

                var projetoComCriador = new
                {
                    projeto.Id_projetos,
                    projeto.Titulo_projetos,
                    projeto.Preco,
                    projeto.Descricao_projeto,
                    NomeCriador = projeto.Utilizador.Nome
                };

                return Ok(projetoComCriador);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao buscar o projeto: {ex.Message}");
            }
        }

        [Authorize] 
        [HttpGet("meusprojetos")]
        public async Task<IActionResult> ObterProjetosContratados()
        {
            try
            {

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


                var projetosContratados = await _context.Contratacoes
                    .Where(c => c.Id_utilizador == userId)  
                    .Include(c => c.Projeto) 
                    .ThenInclude(p => p.Utilizador)
                    .ToListAsync();

                if (projetosContratados == null || !projetosContratados.Any())
                {
                    return NotFound(new { Message = "Nenhum projeto encontrado." });
                }

                return Ok(projetosContratados.Select(c => new
                {
                    c.Id_contratacao,
                    c.Projeto.Titulo_projetos,
                    c.Projeto.Descricao_projeto,
                    c.Projeto.Preco,
                    c.Status_contratacao,
                    Cliente = c.Projeto.Utilizador.Nome + " " + c.Projeto.Utilizador.Sobrenome
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Erro ao processar a solicitação: " + ex.Message });
            }
        }

    }
}
