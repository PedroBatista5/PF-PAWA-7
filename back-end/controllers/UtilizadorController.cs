using Backend.Services;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Register([FromForm] Utilizador utilizador, IFormFile imagemPerfil)
        {
            if (utilizador == null)
            {
                return BadRequest(new { Message = "Dados inválidos." });
            }

            if (string.IsNullOrEmpty(utilizador.TipoUtilizador))
            {
                return BadRequest(new { Message = "Tipo de utilizador é obrigatório." });
            }

            if (utilizador.TipoUtilizador != "Freelancer" && utilizador.TipoUtilizador != "Cliente")
            {
                return BadRequest(new { Message = "Tipo de utilizador inválido. Deve ser 'Freelancer' ou 'Cliente'." });
            }

            if (utilizador.TipoUtilizador == "Freelancer" && (string.IsNullOrEmpty(utilizador.Descricao_info) || string.IsNullOrEmpty(utilizador.Servicos)))
            {
                return BadRequest(new { Message = "Campos adicionais de Freelancer são obrigatórios." });
            }

            // Lógica para tratar a imagem
            if (imagemPerfil != null)
            {
                // Processar a imagem
                var imagePath = Path.Combine("C:\\path\\to\\save", imagemPerfil.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imagemPerfil.CopyToAsync(stream);
                }
                utilizador.Imagem_perfil = imagePath;
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
