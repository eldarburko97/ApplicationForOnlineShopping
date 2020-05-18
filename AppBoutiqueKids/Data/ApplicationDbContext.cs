using System;
using System.Collections.Generic;
using System.Text;
using AppBoutiqueKids.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppBoutiqueKids.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,Role,int>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Brand> Brands { get; set; }
    
        public DbSet<Category> Categories { get; set; }
        
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
       
        public DbSet<SupplierProducts> Supplier_Products { get; set; }
        public DbSet<Supplier_Shipper> Supplier_Shipper { get; set; }
    }

}
