using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TareaCorta4API.DataAccess.Models;

public partial class Semana3PvContext : DbContext
{
    public Semana3PvContext()
    {
    }

    public Semana3PvContext(DbContextOptions<Semana3PvContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<CorreoElectronico> CorreoElectronicos { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Base");

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.Identificacion);

            entity.ToTable("Contacto");

            entity.Property(e => e.Identificacion).ValueGeneratedNever();
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Facebook)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Instagram)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Telefonos)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Twitter)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CorreoElectronico>(entity =>
        {
            entity.HasKey(e => e.Idcontacto);

            entity.ToTable("CorreoElectronico");

            entity.Property(e => e.Idcontacto)
                .ValueGeneratedNever()
                .HasColumnName("IDContacto");
            entity.Property(e => e.Correoelectronico1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Correoelectronico");

            entity.HasOne(d => d.IdcontactoNavigation).WithOne(p => p.CorreoElectronico)
                .HasForeignKey<CorreoElectronico>(d => d.Idcontacto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CorreoElectronico_Contacto");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.HasKey(e => e.Idcontacto);

            entity.Property(e => e.Idcontacto)
                .ValueGeneratedNever()
                .HasColumnName("IDContacto");
            entity.Property(e => e.Telefonos)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdcontactoNavigation).WithOne(p => p.Telefono)
                .HasForeignKey<Telefono>(d => d.Idcontacto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Telefonos_Contacto");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Identificacion);

            entity.Property(e => e.Identificacion).ValueGeneratedNever();
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
