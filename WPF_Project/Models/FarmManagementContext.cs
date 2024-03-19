using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace WPF_Project.Models
{
    public partial class FarmManagementContext : DbContext
    {
        public FarmManagementContext()
        {
        }

        public FarmManagementContext(DbContextOptions<FarmManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CareProcess> CareProcesses { get; set; } = null!;
        public virtual DbSet<HarvestProcess> HarvestProcesses { get; set; } = null!;
        public virtual DbSet<PreservationProcess> PreservationProcesses { get; set; } = null!;
        public virtual DbSet<Produce> Produces { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string ConnectionStr = config.GetConnectionString("DB");

                optionsBuilder.UseSqlServer(ConnectionStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CareProcess>(entity =>
            {
                entity.ToTable("CareProcess");

                entity.Property(e => e.CareProcessId).HasColumnName("CareProcessID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProduceId).HasColumnName("ProduceID");

                entity.HasOne(d => d.Produce)
                    .WithMany(p => p.CareProcesses)
                    .HasForeignKey(d => d.ProduceId)
                    .HasConstraintName("FK__CareProce__Produ__398D8EEE");
            });

            modelBuilder.Entity<HarvestProcess>(entity =>
            {
                entity.ToTable("HarvestProcess");

                entity.Property(e => e.HarvestProcessId).HasColumnName("HarvestProcessID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProduceId).HasColumnName("ProduceID");

                entity.HasOne(d => d.Produce)
                    .WithMany(p => p.HarvestProcesses)
                    .HasForeignKey(d => d.ProduceId)
                    .HasConstraintName("FK__HarvestPr__Produ__3C69FB99");
            });

            modelBuilder.Entity<PreservationProcess>(entity =>
            {
                entity.ToTable("PreservationProcess");

                entity.Property(e => e.PreservationProcessId).HasColumnName("PreservationProcessID");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ProduceId).HasColumnName("ProduceID");

                entity.HasOne(d => d.Produce)
                    .WithMany(p => p.PreservationProcesses)
                    .HasForeignKey(d => d.ProduceId)
                    .HasConstraintName("FK__Preservat__Produ__3F466844");
            });

            modelBuilder.Entity<Produce>(entity =>
            {
                entity.ToTable("Produce");

                entity.Property(e => e.ProduceId).HasColumnName("ProduceID");

                entity.Property(e => e.Season).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
