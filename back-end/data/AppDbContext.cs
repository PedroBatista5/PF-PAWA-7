using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Utilizador> Utilizadores { get; set; }
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

            entity.Property(u => u.TipoUtilizador).IsRequired();
            entity.Property(u => u.Descricao_info).HasMaxLength(500); 
            entity.Property(u => u.Servicos).HasMaxLength(200); 
        });

        modelBuilder.Entity<Projetos>(entity =>
        {
            entity.HasKey(p => p.Id_projetos);
            entity.Property(p => p.Id_projetos).ValueGeneratedOnAdd();
            entity.Property(p => p.Titulo_projetos).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Preco).IsRequired();
            entity.Property(p => p.Descricao_projeto).IsRequired().HasMaxLength(200);
        });
    }
}
