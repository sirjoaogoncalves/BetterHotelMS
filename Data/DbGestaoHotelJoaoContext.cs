using GestaoHotelJoao.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoHotelJoao.Data;

public partial class DbGestaoHotelJoaoContext : DbContext
{
    public DbGestaoHotelJoaoContext()
    {
    }

    public DbGestaoHotelJoaoContext(DbContextOptions<DbGestaoHotelJoaoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Funcionario> Funcionarios { get; set; }

    public virtual DbSet<Quarto> Quartos { get; set; }

    public virtual DbSet<Registo> Registos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.Nome).HasMaxLength(255);
        });

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.ToTable("Funcionario");

            entity.Property(e => e.Nome).HasMaxLength(255);
        });

        modelBuilder.Entity<Quarto>(entity =>
        {
            entity.ToTable("Quarto");

            entity.Property(e => e.CustoNoite).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TipoQuarto).HasMaxLength(255);
        });

        modelBuilder.Entity<Registo>(entity =>
        {
            entity.ToTable("Registo");

            entity.HasIndex(e => e.ClienteId, "IX_Registo_ClienteID");

            entity.HasIndex(e => e.FuncionarioId, "IX_Registo_FuncionarioID");

            entity.HasIndex(e => e.QuartoId, "IX_Registo_QuartoID");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.FuncionarioId).HasColumnName("FuncionarioID");
            entity.Property(e => e.QuartoId).HasColumnName("QuartoID");
            entity.Property(e => e.TotalDiasEstadia).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Registos).HasForeignKey(d => d.ClienteId);

            entity.HasOne(d => d.Funcionario).WithMany(p => p.Registos).HasForeignKey(d => d.FuncionarioId);

            entity.HasOne(d => d.Quarto).WithMany(p => p.Registos).HasForeignKey(d => d.QuartoId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FE69CE724");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Administrador).HasColumnName("administrador");
            entity.Property(e => e.Funcionario).HasColumnName("funcionario");
            entity.Property(e => e.Senha)
                .HasMaxLength(100)
                .HasColumnName("senha");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
