using Microsoft.AspNetCore.Mvc;
using SeuProjeto.Services;
namespace SeuProjeto.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ContratacaoController : ControllerBase
{
    private readonly IContratacaoService _contratacaoService;

    public ContratacaoController(IContratacaoService contratacaoService)
    {
        _contratacaoService = contratacaoService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarContratacao([FromBody] Contratacao contratacao)
    {
        Console.WriteLine($"Dados recebidos: {System.Text.Json.JsonSerializer.Serialize(contratacao)}");

        if (contratacao == null || contratacao.Id_utilizador == 0 || contratacao.Id_projetos == 0)
        {
            return BadRequest("Id_utilizador e Id_projetos são obrigatórios.");
        }

        try
        {
            var novaContratacao = await _contratacaoService.CriarContratacaoAsync(
                contratacao.Id_utilizador,
                contratacao.Id_projetos,
                contratacao.Status_contratacao
            );

            return Ok(new { Message = "Contratação criada com sucesso!", contratacao = novaContratacao });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro no servidor: " + ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarStatus(int id, [FromBody] Contratacao contratacaoAtualizada)
    {
        if (contratacaoAtualizada == null || id != contratacaoAtualizada.Id_contratacao)
        {
            return BadRequest("Dados inválidos.");
        }

        var contratacaoExistente = await _contratacaoService.ObterContratacaoPorIdAsync(id);
        
        if (contratacaoExistente == null)
        {
            return NotFound("Contratação não encontrada.");
        }

        await _contratacaoService.AtualizarContratacaoAsync(contratacaoAtualizada);

        return Ok("Status atualizado com sucesso.");
    }




}

