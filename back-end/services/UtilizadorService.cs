using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Backend.Services
{
    public class UtilizadorService
    {
        private readonly AppDbContext _context;

        public UtilizadorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> RegistrarUtilizadorAsync(Utilizador utilizador)
        {
            if (await _context.Utilizadores.AnyAsync(u => u.Email == utilizador.Email))
            {
                return (false, "O email já está em uso.");
            }

            utilizador.Password = BCrypt.Net.BCrypt.HashPassword(utilizador.Password); 
            await _context.Utilizadores.AddAsync(utilizador);
            await _context.SaveChangesAsync();

            return (true, "Usuário registrado com sucesso.");
        }

        public async Task<List<Utilizador>> ObterTodosUtilizadoresAsync()
        {
            return await _context.Utilizadores.ToListAsync();
        }
    }
}
