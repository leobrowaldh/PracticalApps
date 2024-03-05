using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Packt.Shared;

public partial class NorthwindContext : DbContext
{
    public NorthwindContext()
    {
    }

    public NorthwindContext(DbContextOptions<NorthwindContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Shipper> Shippers { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Territory> Territories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   
        //To make it work in both vs and vsCode:
        if (!optionsBuilder.IsConfigured)
        {
            string dir = Environment.CurrentDirectory;
            string path;
            if (dir.EndsWith("net8.0"))
            {
                // Running in the <project>\bin\<Debug|Release>\net8.0 directory.
                path = Path.Combine("..", "..", "..", "..", "Northwind.db");
            }
            else
            {
                // Running in the <project> directory.
                path = Path.Combine("..", "Northwind.db");
            }

            optionsBuilder.UseSqlite($"Filename={path}");
        }
    }

    //Marks corrected version:
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductId });

            entity.HasOne(d => d.Order)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(d => d.OrderId)
                      .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product)
                      .WithMany(p => p.OrderDetails)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Product>()
          .Property(product => product.UnitPrice)
          .HasConversion<double>();

        OnModelCreatingPartial(modelBuilder);
    }




    ////original scaffolded
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<OrderDetail>(entity =>
    //    {
    //        entity.Property(e => e.Quantity).HasDefaultValue((short)1);

    //        entity.HasOne(d => d.Order)
    //        .WithMany(p => p.OrderDetails)
    //        .OnDelete(DeleteBehavior.ClientSetNull);

    //        entity.HasOne(d => d.Product)
    //        .WithMany(p => p.OrderDetails)
    //        .OnDelete(DeleteBehavior.ClientSetNull);
    //    });

    //    modelBuilder.Entity<Product>(entity =>
    //    {
    //        entity.Property(e => e.ReorderLevel).HasDefaultValue((short)0);
    //        entity.Property(e => e.UnitPrice).HasDefaultValue(0.0);
    //        entity.Property(e => e.UnitsInStock).HasDefaultValue((short)0);
    //        entity.Property(e => e.UnitsOnOrder).HasDefaultValue((short)0);
    //    });

    //    OnModelCreatingPartial(modelBuilder);
    //}

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
