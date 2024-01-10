using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Labb1SQL.Models;

namespace Labb1SQL.Data
{
    public partial class SkolaDBContext : DbContext
    {
        public SkolaDBContext()
        {
        }

        public SkolaDBContext(DbContextOptions<SkolaDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Betyg> Betygs { get; set; } = null!;
        public virtual DbSet<Elever> Elevers { get; set; } = null!;
        public virtual DbSet<Klasser> Klassers { get; set; } = null!;
        public virtual DbSet<Kurser> Kursers { get; set; } = null!;
        public virtual DbSet<Personal> Personals { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SkolaDB;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Betyg>(entity =>
            {
                entity.ToTable("Betyg");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Grade).HasColumnName("grade");
            });

            modelBuilder.Entity<Elever>(entity =>
            {
                entity.ToTable("Elever");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassId).HasColumnName("class_id");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.GradeId).HasColumnName("grade_id");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");
            });

            modelBuilder.Entity<Klasser>(entity =>
            {
                entity.ToTable("Klasser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("class_name");
            });

            modelBuilder.Entity<Kurser>(entity =>
            {
                entity.ToTable("Kurser");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("course_name");
            });

            modelBuilder.Entity<Personal>(entity =>
            {
                entity.ToTable("Personal");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Category)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("category");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("last_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
