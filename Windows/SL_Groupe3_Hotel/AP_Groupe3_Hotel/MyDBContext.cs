using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AP_Groupe3_Hotel.Models;

public partial class MyDBContext : DbContext
{
    public MyDBContext()
    {
    }

    public MyDBContext(DbContextOptions<MyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbChambre> TbChambres { get; set; }

    public virtual DbSet<TbClient> TbClients { get; set; }

    public virtual DbSet<TbEtage> TbEtages { get; set; }

    public virtual DbSet<TbLocalite> TbLocalites { get; set; }

    public virtual DbSet<TbReservation> TbReservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string chaineConnexion = ConfigurationManager.ConnectionStrings["MaConnexion"].ConnectionString;
        MySqlConnection connexion = new MySqlConnection(chaineConnexion);

        optionsBuilder.UseMySql(chaineConnexion, ServerVersion.Parse("5.7.30-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<TbChambre>(entity =>
        {
            entity.HasKey(e => new { e.PkCha, e.PfkChaEta })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tb_chambres");

            entity.HasIndex(e => e.PfkChaEta, "ind_fk_cha_eta");

            entity.Property(e => e.PkCha)
                .ValueGeneratedOnAdd()
                .HasColumnType("int(11)")
                .HasColumnName("pk_cha");
            entity.Property(e => e.PfkChaEta)
                .HasColumnType("int(11)")
                .HasColumnName("pfk_cha_eta");
            entity.Property(e => e.CapCha)
                .HasMaxLength(45)
                .HasColumnName("cap_cha");
            entity.Property(e => e.CodeCha)
                .HasColumnType("int(11)")
                .HasColumnName("code_cha");
            entity.Property(e => e.PrixCha)
                .HasPrecision(10, 2)
                .HasColumnName("prix_cha");

            entity.HasOne(d => d.PfkChaEtaNavigation).WithMany(p => p.TbChambres)
                .HasForeignKey(d => d.PfkChaEta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tb_chambres_tb_etages1");
        });

        modelBuilder.Entity<TbClient>(entity =>
        {
            entity.HasKey(e => e.PkCli).HasName("PRIMARY");

            entity.ToTable("tb_clients");

            entity.HasIndex(e => e.FkCliLoc, "ind_fk_cli_loc");

            entity.Property(e => e.PkCli)
                .HasColumnType("int(11)")
                .HasColumnName("pk_cli");
            entity.Property(e => e.DatNaisCli).HasColumnName("datNais_cli");
            entity.Property(e => e.FkCliLoc)
                .HasColumnType("int(11)")
                .HasColumnName("fk_cli_loc");
            entity.Property(e => e.MailCli)
                .HasMaxLength(100)
                .HasColumnName("mail_cli");
            entity.Property(e => e.NomCli)
                .HasMaxLength(45)
                .HasColumnName("nom_cli");
            entity.Property(e => e.PreCli)
                .HasMaxLength(45)
                .HasColumnName("pre_cli");
            entity.Property(e => e.RueCli)
                .HasMaxLength(100)
                .HasColumnName("rue_cli");
            entity.Property(e => e.TelCli)
                .HasMaxLength(45)
                .HasColumnName("tel_cli");

            entity.HasOne(d => d.FkCliLocNavigation).WithMany(p => p.TbClients)
                .HasForeignKey(d => d.FkCliLoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tb_clients_tb_localites");
        });

        modelBuilder.Entity<TbEtage>(entity =>
        {
            entity.HasKey(e => e.PkEta).HasName("PRIMARY");

            entity.ToTable("tb_etages");

            entity.HasIndex(e => e.CodeEta, "code_eta_UNIQUE").IsUnique();

            entity.Property(e => e.PkEta)
                .HasColumnType("int(11)")
                .HasColumnName("pk_eta");
            entity.Property(e => e.CodeEta)
                .HasColumnType("int(11)")
                .HasColumnName("code_eta");
        });

        modelBuilder.Entity<TbLocalite>(entity =>
        {
            entity.HasKey(e => e.PkLoc).HasName("PRIMARY");

            entity.ToTable("tb_localites");

            entity.Property(e => e.PkLoc)
                .HasColumnType("int(11)")
                .HasColumnName("pk_loc");
            entity.Property(e => e.NomLoc)
                .HasMaxLength(100)
                .HasColumnName("nom_loc");
            entity.Property(e => e.NpaLoc)
                .HasMaxLength(10)
                .HasColumnName("npa_loc");
        });

        modelBuilder.Entity<TbReservation>(entity =>
        {
            entity.HasKey(e => e.PkRes).HasName("PRIMARY");

            entity.ToTable("tb_reservations");

            entity.HasIndex(e => new { e.FkResCha, e.FkResChaEta }, "ind_fk_res_cha");

            entity.HasIndex(e => e.FkResCli, "ind_fk_res_cli");

            entity.Property(e => e.PkRes)
                .HasColumnType("int(11)")
                .HasColumnName("pk_res");
            entity.Property(e => e.DatArrRes).HasColumnName("datArr_res");
            entity.Property(e => e.DatDepRes).HasColumnName("datDep_res");
            entity.Property(e => e.DatJouRes)
                .HasColumnType("datetime")
                .HasColumnName("datJou_res");
            entity.Property(e => e.DejRes)
                .HasColumnType("tinyint(4)")
                .HasColumnName("dej_res");
            entity.Property(e => e.FkResCha)
                .HasColumnType("int(11)")
                .HasColumnName("fk_res_cha");
            entity.Property(e => e.FkResChaEta)
                .HasColumnType("int(11)")
                .HasColumnName("fk_res_cha_eta");
            entity.Property(e => e.FkResCli)
                .HasColumnType("int(11)")
                .HasColumnName("fk_res_cli");
            entity.Property(e => e.NbrPerRes)
                .HasMaxLength(45)
                .HasColumnName("nbrPer_res");

            entity.HasOne(d => d.FkResCliNavigation).WithMany(p => p.TbReservations)
                .HasForeignKey(d => d.FkResCli)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tb_reservations_tb_clients1");

            entity.HasOne(d => d.TbChambre).WithMany(p => p.TbReservations)
                .HasForeignKey(d => new { d.FkResCha, d.FkResChaEta })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tb_reservations_tb_chambres1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
