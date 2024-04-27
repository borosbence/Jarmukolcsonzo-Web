using Jarmukolcsonzo.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Jarmukolcsonzo.Web.Data;

public partial class JKContext : DbContext
{
    public JKContext()
    {
    }

    public JKContext(DbContextOptions<JKContext> options)
        : base(options)
    {
    }

    public virtual DbSet<JarmuTipus> jarmu_tipusok { get; set; }

    public virtual DbSet<Jarmu> jarmuvek { get; set; }

    public virtual DbSet<Rendeles> rendelesek { get; set; }

    public virtual DbSet<Ugyfel> ugyfelek { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user id=root;database=jarmukolcsonzo", ServerVersion.Parse("10.4.28-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<JarmuTipus>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Jarmu>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasOne(d => d.tipus).WithMany(p => p.jarmuvek)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jarmuvek_ibfk_1");
        });

        modelBuilder.Entity<Rendeles>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.HasOne(d => d.jarmu).WithMany(p => p.rendelesek)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rendelesek_ibfk_2");

            entity.HasOne(d => d.ugyfel).WithMany(p => p.rendelesek)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rendelesek_ibfk_1");
        });

        modelBuilder.Entity<Ugyfel>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
