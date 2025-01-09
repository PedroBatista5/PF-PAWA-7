using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilizadorController : ControllerBase
    {
        private readonly UtilizadorService _service;

        public UtilizadorController(UtilizadorService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Utilizador utilizador)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos.");
            }

            var (success, message) = await _service.RegistrarUtilizadorAsync(utilizador);

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(new { message });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var utilizadores = await _service.ObterTodosUtilizadoresAsync();

            if (utilizadores == null || !utilizadores.Any())
            {
                return NoContent();
            }

            return Ok(utilizadores);
        }
    }
}
