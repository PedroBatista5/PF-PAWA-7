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
    // Verifique os dados recebidos
    Console.WriteLine($"Dados recebidos: {System.Text.Json.JsonSerializer.Serialize(contratacao)}");

    if (contratacao == null || contratacao.Id_utilizador == 0 || contratacao.Id_projetos == 0)
    {
        return BadRequest("Cliente e Projeto são obrigatórios.");
    }

    try
    {
        // Continue com a lógica de criação da contratação
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

}

