using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using eMeterApi.Entities;

namespace eMeterApi.Data;

public partial class EMeterContext : DbContext
{
    public EMeterContext()
    {
    }

    public EMeterContext(DbContextOptions<EMeterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MeterDataTable> MeterDataTables { get; set; }

    public virtual DbSet<SysProyecto> SysProyectos { get; set; }

    public virtual DbSet<SysProyectoUsuario> SysProyectoUsuarios { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:eMeter");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MeterDataTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("MeterDataTable");

            entity.Property(e => e.Battery).HasMaxLength(255);
            entity.Property(e => e.Battery1).HasMaxLength(255);
            entity.Property(e => e.CfUnit).HasMaxLength(255);
            entity.Property(e => e.CfUnitSetDay).HasMaxLength(255);
            entity.Property(e => e.CheckSum).HasMaxLength(255);
            entity.Property(e => e.ControlCode).HasMaxLength(255);
            entity.Property(e => e.DataId).HasMaxLength(255);
            entity.Property(e => e.DevDate).HasMaxLength(255);
            entity.Property(e => e.DevTime).HasMaxLength(255);
            entity.Property(e => e.Eealarm)
                .HasMaxLength(255)
                .HasColumnName("EEAlarm");
            entity.Property(e => e.Empty).HasMaxLength(255);
            entity.Property(e => e.EndMark).HasMaxLength(255);
            entity.Property(e => e.FlowRateUnit).HasMaxLength(255);
            entity.Property(e => e.GroupId).IsUnicode(false);
            entity.Property(e => e.MeterAddress).HasMaxLength(255);
            entity.Property(e => e.MeterType).HasMaxLength(255);
            entity.Property(e => e.OverRange).HasMaxLength(255);
            entity.Property(e => e.RegistrationDate).HasColumnType("datetime");
            entity.Property(e => e.Reserved).HasMaxLength(255);
            entity.Property(e => e.ReverseCfUnit).HasMaxLength(255);
            entity.Property(e => e.ReverseFlow).HasMaxLength(255);
            entity.Property(e => e.Ser).HasMaxLength(255);
            entity.Property(e => e.StartCode).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Valve).HasMaxLength(255);
            entity.Property(e => e.WaterTemp).HasMaxLength(255);
        });

        modelBuilder.Entity<SysProyecto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_sys_Proyectos");

            entity.ToTable("Sys_Proyectos", "Global");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Clave)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Proyecto)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("proyecto");
            entity.Property( e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
        });

        modelBuilder.Entity<SysProyectoUsuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sys_Proy__3213E83FD9A37A96");

            entity.ToTable("Sys_ProyectoUsuario", "Global");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProyecto).HasColumnName("id_proyecto");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

            entity.HasOne(d => d.IdProyectoNavigation).WithMany(p => p.SysProyectoUsuarios)
                .HasForeignKey(d => d.IdProyecto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProyectoUsuario_Proyecto");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.SysProyectoUsuarios)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProyectoUsuario_Usuario");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_sys_usuario");

            entity.ToTable("Usuarios", "Global");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Empresa)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("empresa");
            entity.Property(e => e.Operador)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("operador");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
