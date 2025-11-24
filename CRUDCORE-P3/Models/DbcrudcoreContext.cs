using CRUDCORE_P3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CRUDCORE_P3_Prueva.Models;

public partial class DbcrudcoreContext : DbContext
{
    public DbcrudcoreContext()
    {
    }

    public DbcrudcoreContext(DbContextOptions<DbcrudcoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cargo> Cargos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cargo>(entity =>
        {
            entity.HasKey(e => e.IdCargo).HasName("PK__CARGO__6C985625E731EFBF");

            entity.ToTable("CARGO");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__EMPLEADO__CE6D8B9EA86FAE80");

            entity.ToTable("EMPLEADO");

            entity.Property(e => e.Correo)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.HasOne(d => d.oCargo).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdCargo)
                .HasConstraintName("FK_Cargo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
