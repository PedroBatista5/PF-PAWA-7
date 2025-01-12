using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UtilizadorController : ControllerBase
{
    private readonly UtilizadorService _utilizadorService;

    public UtilizadorController(UtilizadorService utilizadorService)
    {
        _utilizadorService = utilizadorService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(new { Message = "Endpoint funcionando!" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] Utilizador utilizador)
    {
        if (utilizador == null)
        {
            return BadRequest(new { Message = "Dados inválidos." });
        }

        var (success, message) = await _utilizadorService.RegistrarUtilizadorAsync(utilizador);

        if (success)
        {
            return Ok(new { Message = message });
        }

        return BadRequest(new { Message = message });
    }
}
