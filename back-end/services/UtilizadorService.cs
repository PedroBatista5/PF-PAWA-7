using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

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


    public async Task<(bool success, string message)> LoginAsync(string email, string password)
    {
        // Procurar o utilizador pelo email
        var utilizador = await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.Email == email);

        // Verificar se o utilizador existe
        if (utilizador == null)
        {
            return (false, "Usuário não encontrado.");
        }

        // Verificar a senha
        bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, utilizador.Password);
        if (!passwordMatches)
        {
            return (false, "Senha incorreta.");
        }

        return (true, "Login bem-sucedido.");
    }

    public async Task<Utilizador> ObterUsuarioPorNomeAsync(string nome)
    {
        return (await _context.Utilizadores
            .FirstOrDefaultAsync(u => u.Nome == nome))!;

    }

    public async Task<(bool success, string message)> AtualizarUsuarioAsync(Utilizador utilizador)
    {
        _context.Utilizadores.Update(utilizador);
        await _context.SaveChangesAsync();

        return (true, "Usuário atualizado com sucesso.");
    }

 }

}
