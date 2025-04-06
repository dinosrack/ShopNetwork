using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopNetwork.Models;

public partial class ShopNetworkContext : DbContext
{
    public ShopNetworkContext()
    {
    }

    public ShopNetworkContext(DbContextOptions<ShopNetworkContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\sqlexpress; Database=ShopNetwork; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PK__Inventor__F5FDE6D30E460A09");

            entity.ToTable("Inventory");

            entity.Property(e => e.InventoryId).HasColumnName("InventoryID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.StoreId).HasColumnName("StoreID");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Produ__73BA3083");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Store__72C60C4A");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED1665A5AE");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK__Stores__3B82F0E1C1006404");

            entity.Property(e => e.StoreId).HasColumnName("StoreID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.DirectorLastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC018A9FEB");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E44E1C0368").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserPassword).HasMaxLength(100);
            entity.Property(e => e.UserRole).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
