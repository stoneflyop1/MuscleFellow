﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MuscleFellow.Models.Domain;

namespace MuscleFellow.Models
{
    public class MuscleFellowDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly bool _created;

        public MuscleFellowDbContext(DbContextOptions options) :base(options)
        {
            if (!_created)
            {
                _created = true;
                Database.EnsureDeleted();
                Database.EnsureCreated();
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Brand>().HasKey(b=>b.BrandID);
            builder.Entity<Category>().HasKey(c=>c.CategoryID);
            builder.Entity<OrderDetail>().HasKey(o=>o.OrderDetailID);
            builder.Entity<Order>().HasKey(o=>o.OrderID);
            builder.Entity<Product>().HasKey(p=>p.ProductID);
            builder.Entity<ShipAddress>().HasKey(a=>a.AddressID);
            builder.Entity<CartItem>().HasKey(c=>c.CartID);
            builder.Entity<ApplicationUser>().HasKey(u=>u.Id);
            builder.Entity<ProductImage>().HasKey(p => p.ImageID);

            base.OnModelCreating(builder);
        }

        public DbSet<Brand> Brands { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ShipAddress> ShipAddresses { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<Province> Provinces { get; set; }

        public DbSet<City> Cities { get; set; }

    }
}
