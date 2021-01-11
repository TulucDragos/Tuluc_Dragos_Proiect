using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tuluc_Dragos_Proiect.Models;

namespace Tuluc_Dragos_Proiect.Data
{
    public class ShopContext: DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Hammock> Hammocks { get; set; }

        public DbSet<Distribuitor> Distribuitors { get; set; }
        public DbSet<DistributedHammock> DistributedHammocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Hammock>().ToTable("Hammock");
            modelBuilder.Entity<Distribuitor>().ToTable("Distribuitor");
            modelBuilder.Entity<DistributedHammock>().ToTable("DistributedHammock");

            modelBuilder.Entity<DistributedHammock>().HasKey(c => new { c.HammockID, c.DistribuitorID });//configureaza cheiavprimara compusa
        }
    }
}
