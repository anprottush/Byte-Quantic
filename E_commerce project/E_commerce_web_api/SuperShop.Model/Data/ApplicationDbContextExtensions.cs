using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SuperShop.Model.DBEntity;
using SuperShop.Model.DBEntity.Customers;
using SuperShop.Model.DBEntity.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SuperShop.Model.Data
{
    public partial class ApplicationDbContext 
    {
        protected void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            #region Global filter for IsRemoved Status
            //modelBuilder.Entity<BaseEntity>().HasQueryFilter(b => b.IsRemoved);

            // define your filter expression tree
            Expression<Func<BaseEntity, bool>> filterExpr = bm => (bool)!bm.IsRemoved;
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                // check if current entity type is child of BaseModel
                if (mutableEntityType.ClrType.IsAssignableTo(typeof(BaseEntity)))
                {
                    // modify expression to handle correct child type
                    var parameter = Expression.Parameter(mutableEntityType.ClrType);
                    var body = ReplacingExpressionVisitor.Replace(filterExpr.Parameters.First(), parameter, filterExpr.Body);
                    var lambdaExpression = Expression.Lambda(body, parameter);

                    // set filter
                    mutableEntityType.SetQueryFilter(lambdaExpression);
                }
            }
            #endregion

            modelBuilder.Entity<Brand>().HasIndex(e => e.BrandName).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(e => e.CategoryName).IsUnique();
            modelBuilder.Entity<Product>().HasIndex(e => e.ProductName).IsUnique();

            modelBuilder.Entity<Order>().HasIndex(e => e.OrderNo).IsUnique();
        }
    }
}
