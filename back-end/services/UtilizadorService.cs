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

            // Criptografar a senha
            utilizador.Password = BCrypt.Net.BCrypt.HashPassword(utilizador.Password);

            // Caso o utilizador seja Freelancer, podemos adicionar campos adicionais diretamente no Utilizador
            if (utilizador.TipoUtilizador == "Freelancer")
            {
                // Preenchendo os campos adicionais diretamente no modelo Utilizador
                utilizador.Descricao_info = "";   // Campo vazio
                utilizador.Servicos = "";         // Campo vazio
                utilizador.Imagem_perfil = null;  // Campo vazio (ou imagem padrão)
            }

            // Adicionar o usuário à tabela Utilizadores
            await _context.Utilizadores.AddAsync(utilizador);
            await _context.SaveChangesAsync(); // Salva o usuário

            return (true, "Usuário e informações do perfil criados com sucesso.");
        }

        internal async Task<(bool success, string message)> LoginAsync(string email, string password)
        {
            // Implementar a lógica de login (não implementado no código atual)
            throw new NotImplementedException();
        }
    }
}
