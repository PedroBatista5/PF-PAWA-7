using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Utilizador> Utilizadores { get; set; }
    public DbSet<Projetos> Projetos { get; set; }
    public DbSet<Contratacao> Contratacoes { get; set; } // Adicionado

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da entidade Utilizador
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
            entity.ToTable("projetos"); // Nome da tabela no banco

            entity.HasKey(p => p.Id_projetos); // Chave primária
            entity.Property(p => p.Id_projetos)
                .ValueGeneratedOnAdd(); // Auto incremento

            entity.Property(p => p.Titulo_projetos)
                .IsRequired()
                .HasMaxLength(100); // Restrição de tamanho como no banco

            entity.Property(p => p.Preco)
                .IsRequired(); // Campo obrigatório

            entity.Property(p => p.Descricao_projeto)
                .IsRequired()
                .HasMaxLength(200); // Restrição de tamanho como no banco

            entity.Property(p => p.Id_utilizador)
                .IsRequired(); // Campo obrigatório

            // Relacionamento com a tabela Utilizadores
            entity.HasOne(p => p.Utilizador)
                .WithMany()
                .HasForeignKey(p => p.Id_utilizador)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete para integridade referencial
        });

        // Configuração da entidade Contratacao
        modelBuilder.Entity<Contratacao>(entity =>
        {
            entity.HasKey(c => c.Id_contratacao);
            entity.Property(c => c.Id_contratacao).ValueGeneratedOnAdd();

            entity.Property(c => c.Data_contratacao).IsRequired();
            entity.Property(c => c.Status_contratacao).IsRequired();

            entity.HasOne(c => c.Projeto)
                .WithMany()
                .HasForeignKey(c => c.Id_projetos) 
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.Id_utilizador) 
                .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
