using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CodeFirst.Models
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("MyConnectionString") { }
        public DbSet<Brand> Brands{get;set ;}

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }

    }
}