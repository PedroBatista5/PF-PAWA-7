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

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateModel model)
        {
            var user = await _utilizadorService.ObterUsuarioPorNomeAsync(User.Identity.Name);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            user.Descricao_info = model.Info;
            user.Servicos = model.Servicos;

            var (success, message) = await _utilizadorService.AtualizarUsuarioAsync(user);

            if (success)
            {
                return Ok("Perfil atualizado com sucesso.");
            }

            return BadRequest("Erro ao atualizar o perfil.");
        }


    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ProfileUpdateModel
    {
        public string Info { get; set; }
        public string Servicos { get; set; }
    }


}
