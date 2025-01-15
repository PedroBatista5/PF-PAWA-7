using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Register([FromForm] Utilizador utilizador, [FromForm] IFormFile? imagemPerfil)
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

            // Validações específicas para Freelancer
            if (utilizador.TipoUtilizador == "Freelancer")
            {
                if (string.IsNullOrEmpty(utilizador.Descricao_info) || string.IsNullOrEmpty(utilizador.Servicos))
                {
                    return BadRequest(new { Message = "Campos adicionais de Freelancer são obrigatórios." });
                }

                if (imagemPerfil == null)
                {
                    return BadRequest(new { Message = "A imagem de perfil é obrigatória para Freelancers." });
                }

                // Definir um diretório válido para salvar a imagem
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Criar o diretório caso não exista
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                // Caminho completo para salvar a imagem
                var imagePath = Path.Combine(uploadDirectory, imagemPerfil.FileName);

                // Salvar a imagem no diretório
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await imagemPerfil.CopyToAsync(stream);
                }

                // Definir o caminho da imagem no banco de dados
                utilizador.Imagem_perfil = Path.Combine("uploads", imagemPerfil.FileName);  // Caminho relativo
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
