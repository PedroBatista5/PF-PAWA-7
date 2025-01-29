using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class UtilizadorService
    {
        private readonly AppDbContext _context;

        public UtilizadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> RegistrarUtilizadorAsync(Utilizador utilizador)
        {
        // Verificar se o email já existe
        var emailExists = await _context.Utilizadores
            .AnyAsync(u => u.Email == utilizador.Email);

        if (emailExists)
        {
            return (false, "E-mail já registrado.");
        }

        // Verificar se o tipo de utilizador foi fornecido e é válido
        if (string.IsNullOrEmpty(utilizador.TipoUtilizador) || 
            (utilizador.TipoUtilizador != "Freelancer" && utilizador.TipoUtilizador != "Cliente"))
        {
            return (false, "Tipo de utilizador inválido. Deve ser 'Freelancer' ou 'Cliente'.");
        }

        // Caso seja Freelancer, verificar os campos obrigatórios
        if (utilizador.TipoUtilizador == "Freelancer")
        {
            if (string.IsNullOrWhiteSpace(utilizador.Descricao_info) || string.IsNullOrWhiteSpace(utilizador.Servicos))
            {
                return (false, "Por favor, preencha os campos Descrição e Serviços.");
            }
        }

        // Criptografar a senha
        utilizador.Password = BCrypt.Net.BCrypt.HashPassword(utilizador.Password);

        // Adicionar o utilizador à tabela
        await _context.Utilizadores.AddAsync(utilizador);
        await _context.SaveChangesAsync();

        return (true, "Usuário registrado com sucesso.");
    }


    public async Task<(bool success, string message, string token, int id_utilizador)> LoginAsync(string email, string password)
    {
        var utilizador = await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.Email == email);

        if (utilizador == null)
        {
            return (false, "Usuário não encontrado.", null, 0);
        }

        bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, utilizador.Password);
        if (!passwordMatches)
        {
            return (false, "Senha incorreta.", null, 0);
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.UTF8.GetBytes("MINHA_CHAVE_SUPER_SECRETA_QUE_TEM_32_BYTES!"); // Chave de 256 bits

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, utilizador.Id_utilizador.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, utilizador.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return (true, "Login bem-sucedido.", tokenString, utilizador.Id_utilizador);
    }

    public async Task<Utilizador> ObterUsuarioPorNomeAsync(string nome)
    {
        return (await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.Nome == nome))!;
    }

    public async Task<Utilizador> ObterUsuarioPorEmailAsync(string email)
    {
            // Busca o usuário no banco de dados pelo email
            return await _context.Utilizadores.FirstOrDefaultAsync(u => u.Email == email);
    }

        public async Task<string> UpdateProfileAsync(int userId, string descricaoInfo, string servicos)
        {
            var user = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.Id_utilizador == userId);

            if (user == null)
            {
                return "Usuário não encontrado.";
            }

            user.Descricao_info = descricaoInfo;
            user.Servicos = servicos;

            try
            {
                await _context.SaveChangesAsync();
                return "Dados atualizados com sucesso!";
            }
            catch (System.Exception ex)
            {
                return "Erro ao atualizar dados: " + ex.Message;
            }
        }


 }

}
