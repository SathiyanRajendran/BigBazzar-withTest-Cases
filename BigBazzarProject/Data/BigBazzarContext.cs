using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BigBazzar.Models;

namespace BigBazzar.Data
{
    public class BigBazzarContext : DbContext
    {
        public BigBazzarContext()
        {

        }
        public BigBazzarContext (DbContextOptions<BigBazzarContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=AY1KPKYG2U1G;Database=BigBazzar;User Id=sa; Password=!Morning1;MultipleActiveResultSets=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OrderMasters>().HasMany(od => od.OrderDetails).WithOne(om => om.OrderMasters).HasForeignKey(k => k.OrderMasterId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasMany(od => od.OrderDetails).WithOne(pr => pr.Products).HasForeignKey(k => k.ProductId).OnDelete(DeleteBehavior.ClientSetNull);
        }

        public DbSet<BigBazzar.Models.Customers> Customers { get; set; } = default!;
        public DbSet<BigBazzar.Models.Carts> Carts { get; set; } = default!;
        public DbSet<BigBazzar.Models.Feedback> Feedbacks { get; set; } = default!;
        public DbSet<BigBazzar.Models.OrderMasters> OrderMasters { get; set; } = default!;
        public DbSet<BigBazzar.Models.OrderDetails> OrderDetails { get; set; } = default!;
        public DbSet<BigBazzar.Models.Products> Products { get; set; } = default!;
        public DbSet<BigBazzar.Models.Traders> Traders { get; set; } = default!;
        public DbSet<BigBazzar.Models.Categories> Categories { get; set; } = default!;
        public DbSet<BigBazzar.Models.Admin> Admins { get; set; } = default!;

    }
}
