using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<InfoUser> InfoUsers { get; set; }
        public DbSet<Projetos> Projetos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilizador>(entity =>
            {
                entity.HasKey(u => u.Id_utilizador); 
                entity.Property(u => u.Id_utilizador).ValueGeneratedOnAdd();

                entity.Property(u => u.Nome).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Sobrenome).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Password).IsRequired().HasMaxLength(200);
        
                entity.HasOne(u => u.InfoUser)
                      .WithMany(i => i.Utilizadores)
                      .HasForeignKey(u => u.InfoUserId) 
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<InfoUser>(entity =>
            {
                entity.HasKey(i => i.Id_info); 
                entity.Property(i => i.Id_info).ValueGeneratedOnAdd();

                entity.Property(i => i.Descricao_info).HasMaxLength(500);
                entity.Property(i => i.Servicos).HasMaxLength(200);

                entity.HasOne(i => i.Projetos)
                      .WithMany(p => p.InfoUsers)
                      .HasForeignKey(i => i.ProjetosId) 
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Projetos>(entity =>
            {
                entity.HasKey(p => p.Id_projetos); 
                entity.Property(p => p.Id_projetos).ValueGeneratedOnAdd();

                entity.Property(p => p.titulo_projetos).IsRequired().HasMaxLength(100);
                entity.Property(p => p.preco).IsRequired();
                entity.Property(p => p.descricao_projeto).IsRequired().HasMaxLength(200);
            });
        }
    }
}
