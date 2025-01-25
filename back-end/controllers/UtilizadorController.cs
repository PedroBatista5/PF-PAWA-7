using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Register([FromForm] Utilizador utilizador)
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

            // Remova referências à imagem
            if (utilizador.TipoUtilizador == "Freelancer" && (string.IsNullOrEmpty(utilizador.Descricao_info) || string.IsNullOrEmpty(utilizador.Servicos)))
            {
                return BadRequest(new { Message = "Campos adicionais de Freelancer são obrigatórios." });
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


        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userName = User.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Nome de usuário não encontrado no token.");
            }

            var user = await _utilizadorService.ObterUsuarioPorNomeAsync(userName);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            return Ok(new
            {
                Nome = user.Nome,
                Sobrenome = user.Sobrenome,
                TipoUtilizador = user.TipoUtilizador,
                Descricao_info = user.Descricao_info,
                Servicos = user.Servicos
            });
        }


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ProfileUpdateModel
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Descricao_Info { get; set; }
        public string Servicos { get; set; }
    }


}
