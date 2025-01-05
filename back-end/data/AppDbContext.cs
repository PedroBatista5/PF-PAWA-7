using Microsoft.EntityFrameworkCore;
using Backend.Models; 

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilizador> Utilizadores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Utilizador>(entity =>
        {

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();

            entity.Property(u => u.Nome).IsRequired().HasMaxLength(100); 
            entity.Property(u => u.Sobrenome).IsRequired().HasMaxLength(100); 
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique(); 
            entity.Property(u => u.Password).IsRequired().HasMaxLength(200);

        });

        }
    }
}
