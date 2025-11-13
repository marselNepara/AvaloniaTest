using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AvaloniaTest.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SupplierType> SupplierTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=FirstTestAvalonia;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("materials_pkey");

            entity.ToTable("materials");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MaterialTypeId).HasColumnName("material_type_id");
            entity.Property(e => e.MinimumQuantity)
                .HasPrecision(10, 2)
                .HasColumnName("minimum_quantity");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.QuantityInPackage).HasColumnName("quantity_in_package");
            entity.Property(e => e.QuantityInStock)
                .HasPrecision(10, 2)
                .HasColumnName("quantity_in_stock");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(50)
                .HasColumnName("unit_of_measure");
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2)
                .HasColumnName("unit_price");

            entity.HasOne(d => d.MaterialType).WithMany(p => p.Materials)
                .HasForeignKey(d => d.MaterialTypeId)
                .HasConstraintName("materials_material_type_id_fkey");

            entity.HasMany(d => d.Suppliers).WithMany(p => p.Materials)
                .UsingEntity<Dictionary<string, object>>(
                    "MaterialSupplier",
                    r => r.HasOne<Supplier>().WithMany()
                        .HasForeignKey("SupplierId")
                        .HasConstraintName("material_suppliers_supplier_id_fkey"),
                    l => l.HasOne<Material>().WithMany()
                        .HasForeignKey("MaterialId")
                        .HasConstraintName("material_suppliers_material_id_fkey"),
                    j =>
                    {
                        j.HasKey("MaterialId", "SupplierId").HasName("material_suppliers_pkey");
                        j.ToTable("material_suppliers");
                        j.IndexerProperty<int>("MaterialId").HasColumnName("material_id");
                        j.IndexerProperty<int>("SupplierId").HasColumnName("supplier_id");
                    });
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("material_types_pkey");

            entity.ToTable("material_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LossPercentage)
                .HasPrecision(5, 2)
                .HasColumnName("loss_percentage");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_types_pkey");

            entity.ToTable("product_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Coefficient)
                .HasPrecision(10, 4)
                .HasColumnName("coefficient");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Inn)
                .HasMaxLength(20)
                .HasColumnName("inn");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.SupplierTypeId).HasColumnName("supplier_type_id");

            entity.HasOne(d => d.SupplierType).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.SupplierTypeId)
                .HasConstraintName("suppliers_supplier_type_id_fkey");
        });

        modelBuilder.Entity<SupplierType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("supplier_types_pkey");

            entity.ToTable("supplier_types");

            entity.HasIndex(e => e.Name, "supplier_types_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
