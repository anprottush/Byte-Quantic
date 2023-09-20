using SuperShop.Common.Configuration;
using SuperShop.Model.DBEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Model.DBEntity.Customers;
using SuperShop.Model.DBEntity.Others;
using SuperShop.Model.DBEntity.Provider;
using SuperShop.Model.DBEntity.Returns;
using SuperShop.Model.DBEntity.Rider;


namespace SuperShop.Model.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);

            // Some database constratin manage by fluent api, which we make a ApplicationDbContext Extensiona class
            OnModelCreatingPartial(modelBuilder);
        }

        public DbSet<RefreshToken> RefreshToken { get; set; }

        #region Customer
        public DbSet<AddToCart> AddToCart { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CustomerAddress> CustomerAddress { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payment { get; set; }
        #endregion Customer

        #region Others
        public DbSet<Country> Country { get; set; }
        #endregion Others

        #region Product
        public DbSet<Category> Category { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<CategoryImage> CategoryImage { get; set; }
        public DbSet<ProductProvider> ProductProvider { get; set; }
        public DbSet<ProductRating> ProductRating { get; set; }
        #endregion Product

        #region Provider
        public DbSet<ProviderAddress> ProviderAddress { get; set; }
        public DbSet<ProviderInfo> ProviderInfo { get; set; }
        public DbSet<ProviderRating> ProviderRating { get; set; }
        #endregion Provider

        #region Return
        public DbSet<Return> Return { get; set; }
        public DbSet<ReturnDetails> ReturnDetails { get; set; }
        public DbSet<ReturnImage> ReturnImage { get; set; }
        #endregion Return

        #region Rider
        public DbSet<RiderAddress> RiderAddress { get; set; }
        public DbSet<RiderInfo> RiderInfo { get; set; }
        public DbSet<RiderRating> RiderRating { get; set; }
        #endregion Return
    }
}
