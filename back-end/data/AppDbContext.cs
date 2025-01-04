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
                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
            });
        }
    }
}
