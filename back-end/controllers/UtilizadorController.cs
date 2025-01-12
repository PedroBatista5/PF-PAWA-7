using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilizadorController : ControllerBase
    {
        private readonly UtilizadorService _utilizadorService;

        public UtilizadorController(UtilizadorService utilizadorService)
        {
            _utilizadorService = utilizadorService;
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            var (success, message) = await _utilizadorService.LoginAsync(loginModel.Email, loginModel.Password);

            if (success)
            {
                return Ok(new { Message = message });
            }

            return Unauthorized(new { Message = message });
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
