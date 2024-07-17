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
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

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

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CareProcesses)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__CareProce__Produ__398D8EEE");
            });

            modelBuilder.Entity<HarvestProcess>(entity =>
            {
                entity.ToTable("HarvestProcess");

                entity.Property(e => e.HarvestProcessId).HasColumnName("HarvestProcessID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.HarvestProcesses)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__HarvestPr__Produ__3C69FB99");
            });

            modelBuilder.Entity<PreservationProcess>(entity =>
            {
                entity.ToTable("PreservationProcess");

                entity.Property(e => e.PreservationProcessId).HasColumnName("PreservationProcessID");

                entity.Property(e => e.ExpiryDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.PreservationProcesses)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__Preservat__Produ__3F466844");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Season).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.StaffId).HasColumnName("StaffID");

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
