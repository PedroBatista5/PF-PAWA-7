using Backend.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Backend.Data;

namespace Backend.Services
{
    public class UtilizadorService
    {
        private readonly AppDbContext _context;

        public UtilizadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message)> LoginAsync(string email, string password)
        {
            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.Email == email);

            if (utilizador == null)
            {
                return (false, "Usuário não encontrado.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, utilizador.Password))
            {
                return (false, "Senha inválida.");
            }

            return (true, "Login bem-sucedido.");
        }

        public async Task<(bool success, string message)> RegistrarUtilizadorAsync(Utilizador utilizador)
        {
            var emailExists = await _context.Utilizadores
                .AnyAsync(u => u.Email == utilizador.Email);

            if (emailExists)
            {
                return (false, "E-mail já registrado.");
            }

            utilizador.Password = BCrypt.Net.BCrypt.HashPassword(utilizador.Password);

            await _context.Utilizadores.AddAsync(utilizador);
            await _context.SaveChangesAsync();

            return (true, "Usuário registrado com sucesso.");
        }
    }
}
