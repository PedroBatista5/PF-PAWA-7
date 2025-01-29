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
        private readonly AppDbContext _context;
        public UtilizadorController(UtilizadorService utilizadorService, AppDbContext context)
        {
            _utilizadorService = utilizadorService;
            _context = context;
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

            var (success, message, token, id_utilizador) = await _utilizadorService.LoginAsync(loginModel.Email, loginModel.Password);

            if (success)
            {
                return Ok(new { token, id_utilizador });
            }

            return Unauthorized(new { Message = message });
        }



        [HttpGet("getProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest(new { Message = "Email não encontrado no token." });
            }

            var user = await _utilizadorService.ObterUsuarioPorEmailAsync(userEmail);

            if (user == null)
            {
                return NotFound(new { Message = "Utilizador não encontrado." });
            }

            return Ok(new
            {
                user.Nome,
                user.Sobrenome,
                user.TipoUtilizador,
                user.Descricao_info,
                user.Servicos
            });
        }

       [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPerfil(int id, [FromBody] Utilizador utilizador)
        {
            // Verificar se o usuário existe
            var usuarioExistente = await _context.Utilizadores.FindAsync(id);
            if (usuarioExistente == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Atualizar as informações do perfil
            usuarioExistente.Descricao_info = utilizador.Descricao_info;
            usuarioExistente.Servicos = utilizador.Servicos;

            _context.Utilizadores.Update(usuarioExistente);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Perfil atualizado com sucesso!" });
        }


    public class UpdateProfileRequest
    {
        public string Descricao_info { get; set; }
        public string Servicos { get; set; }
    }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    

}
