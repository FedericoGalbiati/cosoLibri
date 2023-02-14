using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using cosoLibri.Models;

namespace cosoLibri.Data;

public partial class LibriDbContext : DbContext
{
    public LibriDbContext(DbContextOptions<LibriDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autore> Autores { get; set; }

    public virtual DbSet<Copium> Copia { get; set; }

    public virtual DbSet<Editore> Editores { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Parolachiave> Parolachiaves { get; set; }

    public virtual DbSet<Prestito> Prestitos { get; set; }

    public virtual DbSet<Socio> Socios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Autore>(entity =>
        {
            entity.HasKey(e => e.CodiceAutore).HasName("PRIMARY");

            entity.ToTable("autore");

            entity.Property(e => e.CodiceAutore).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Cognome)
                .HasMaxLength(45)
                .HasComment("Il cognome potrebbe essere nullo per quegli autori che si firmano con uno pseudonimo");
            entity.Property(e => e.Nazione).HasColumnType("enum('ITALIA','FRANCIA','GERMANIA','INGHILTERRA','USA')");
            entity.Property(e => e.Nome).HasMaxLength(45);
        });

        modelBuilder.Entity<Copium>(entity =>
        {
            entity.HasKey(e => e.NumeroInventario).HasName("PRIMARY");

            entity.ToTable("copia");

            entity.HasIndex(e => e.FkLibro, "fk_copia_libro1_idx");

            entity.Property(e => e.NumeroInventario).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Collocazione)
                .HasDefaultValueSql("'P1ST1SC1'")
                .HasColumnType("enum('P1ST1SC1','P1ST1SC2')");
            entity.Property(e => e.FkLibro).HasColumnType("int(10) unsigned");

            entity.HasOne(d => d.FkLibroNavigation).WithMany(p => p.Copia)
                .HasForeignKey(d => d.FkLibro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_copia_libro1");
        });

        modelBuilder.Entity<Editore>(entity =>
        {
            entity.HasKey(e => e.CodiceEditore).HasName("PRIMARY");

            entity.ToTable("editore");

            entity.Property(e => e.CodiceEditore).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Agente).HasMaxLength(60);
            entity.Property(e => e.Cap)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CAP");
            entity.Property(e => e.Città).HasMaxLength(45);
            entity.Property(e => e.Civico).HasMaxLength(45);
            entity.Property(e => e.Nome).HasMaxLength(45);
            entity.Property(e => e.Provincia).HasMaxLength(45);
            entity.Property(e => e.SitoWeb).HasMaxLength(60);
            entity.Property(e => e.Telefono).HasMaxLength(15);
            entity.Property(e => e.Via).HasMaxLength(45);
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.Codice).HasName("PRIMARY");

            entity.ToTable("libro");

            entity.HasIndex(e => e.Isbn, "ISBN_UNIQUE").IsUnique();

            entity.HasIndex(e => e.FkEditore, "fk_libro_editore1_idx");

            entity.Property(e => e.Codice)
                .ValueGeneratedNever()
                .HasColumnType("int(10) unsigned");
            entity.Property(e => e.AnnoPub)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.FkEditore).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Isbn)
                .HasMaxLength(13)
                .HasColumnName("ISBN");
            entity.Property(e => e.Lingua)
                .HasDefaultValueSql("'IT'")
                .HasColumnType("enum('IT','FN','DE','FR')");
            entity.Property(e => e.Titolo).HasMaxLength(60);

            entity.HasOne(d => d.FkEditoreNavigation).WithMany(p => p.Libros)
                .HasForeignKey(d => d.FkEditore)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_libro_editore1");

            entity.HasMany(d => d.FkAutores).WithMany(p => p.FkLibros)
                .UsingEntity<Dictionary<string, object>>(
                    "Scritto",
                    r => r.HasOne<Autore>().WithMany()
                        .HasForeignKey("FkAutore")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_scritto_autore1"),
                    l => l.HasOne<Libro>().WithMany()
                        .HasForeignKey("FkLibro")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_scritto_libro1"),
                    j =>
                    {
                        j.HasKey("FkLibro", "FkAutore")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("scritto");
                        j.HasIndex(new[] { "FkAutore" }, "fk_scritto_autore1_idx");
                        j.HasIndex(new[] { "FkLibro" }, "fk_scritto_libro1_idx");
                    });
        });

        modelBuilder.Entity<Parolachiave>(entity =>
        {
            entity.HasKey(e => e.IdParola).HasName("PRIMARY");

            entity.ToTable("parolachiave");

            entity.Property(e => e.IdParola).HasColumnType("int(10) unsigned");
            entity.Property(e => e.ParolaChiave1)
                .HasMaxLength(100)
                .HasColumnName("ParolaChiave");

            entity.HasMany(d => d.Fklibros).WithMany(p => p.FkParolaChiaves)
                .UsingEntity<Dictionary<string, object>>(
                    "Contengono",
                    r => r.HasOne<Libro>().WithMany()
                        .HasForeignKey("Fklibro")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Contengono_libro1"),
                    l => l.HasOne<Parolachiave>().WithMany()
                        .HasForeignKey("FkParolaChiave")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_Contengono_ParolaChiave1"),
                    j =>
                    {
                        j.HasKey("FkParolaChiave", "Fklibro")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("contengono");
                        j.HasIndex(new[] { "Fklibro" }, "fk_Contengono_libro1_idx");
                        j.HasIndex(new[] { "FkParolaChiave" }, "fk_Contengono_parolachiave1_idx");
                    });
        });

        modelBuilder.Entity<Prestito>(entity =>
        {
            entity.HasKey(e => new { e.FkSocio, e.FkCopia })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("prestito");

            entity.HasIndex(e => e.FkCopia, "fk_prestito_copia1_idx");

            entity.HasIndex(e => e.FkSocio, "fk_prestito_socio1_idx");

            entity.Property(e => e.FkSocio).HasColumnType("int(10) unsigned");
            entity.Property(e => e.FkCopia).HasColumnType("int(10) unsigned");
            entity.Property(e => e.DataPrestito).HasColumnType("datetime");
            entity.Property(e => e.DataRestituzione).HasColumnType("datetime");

            entity.HasOne(d => d.FkCopiaNavigation).WithMany(p => p.Prestitos)
                .HasForeignKey(d => d.FkCopia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_prestito_copia1");

            entity.HasOne(d => d.FkSocioNavigation).WithMany(p => p.Prestitos)
                .HasForeignKey(d => d.FkSocio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_prestito_socio");
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.HasKey(e => e.Codice).HasName("PRIMARY");

            entity.ToTable("socio");

            entity.HasIndex(e => e.EMail, "E-mail_UNIQUE").IsUnique();

            entity.Property(e => e.Codice).HasColumnType("int(10) unsigned");
            entity.Property(e => e.Cap)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("CAP");
            entity.Property(e => e.Città).HasMaxLength(60);
            entity.Property(e => e.Civico).HasMaxLength(15);
            entity.Property(e => e.Cognome).HasMaxLength(60);
            entity.Property(e => e.EMail)
                .HasMaxLength(60)
                .HasColumnName("E-mail");
            entity.Property(e => e.Nome).HasMaxLength(60);
            entity.Property(e => e.Provincia)
                .HasMaxLength(2)
                .IsFixedLength();
            entity.Property(e => e.Telefono).HasMaxLength(15);
            entity.Property(e => e.Via).HasMaxLength(60);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
