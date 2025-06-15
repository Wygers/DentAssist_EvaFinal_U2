using DentAssist.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Odontologo> Odontologos { get; set; }
    public DbSet<Turno> Turnos { get; set; }
    public DbSet<Tratamiento> Tratamientos { get; set; }
    public DbSet<PlanTratamiento> PlanesTratamiento { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración para Paciente
        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Rut).IsRequired().HasMaxLength(12);
            entity.Property(p => p.Telefono).HasMaxLength(20);
            entity.Property(p => p.Email).HasMaxLength(100);
            entity.Property(p => p.Direccion).HasMaxLength(200);

            entity.HasMany(p => p.Turnos)
                .WithOne(t => t.Paciente)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.PlanesTratamiento)
                .WithOne(pt => pt.Paciente)
                .HasForeignKey(pt => pt.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración para Odontologo
        modelBuilder.Entity<Odontologo>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(o => o.Matricula).IsRequired().HasMaxLength(20);
            entity.Property(o => o.Especialidad).HasMaxLength(50);
            entity.Property(o => o.Email).HasMaxLength(100);

            entity.HasMany(o => o.Turnos)
                .WithOne(t => t.Odontologo)
                .HasForeignKey(t => t.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(o => o.PlanesTratamiento)
                .WithOne(pt => pt.Odontologo)
                .HasForeignKey(pt => pt.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración para Turno
        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.FechaHora).IsRequired();
            entity.Property(t => t.DuracionMinutos).IsRequired().HasDefaultValue(30);
            entity.Property(t => t.Estado).HasMaxLength(20).HasDefaultValue("Pendiente");

            entity.HasOne(t => t.Paciente)
                .WithMany(p => p.Turnos)
                .HasForeignKey(t => t.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Odontologo)
                .WithMany(o => o.Turnos)
                .HasForeignKey(t => t.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración para Tratamiento
        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Descripcion).HasMaxLength(500);
            entity.Property(t => t.PrecioEstimado).IsRequired().HasColumnType("int");
        });

        // Configuración para PlanTratamiento
        modelBuilder.Entity<PlanTratamiento>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.FechaCreacion).HasDefaultValueSql("GETDATE()");

            entity.HasOne(p => p.Paciente)
                .WithMany(p => p.PlanesTratamiento)
                .HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Odontologo)
                .WithMany(o => o.PlanesTratamiento)
                .HasForeignKey(p => p.OdontologoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
