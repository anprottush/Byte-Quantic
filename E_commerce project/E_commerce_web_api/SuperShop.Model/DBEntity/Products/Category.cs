using SuperShop.Model.Data;
using SuperShop.Model.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [Unique(typeof(ApplicationDbContext), nameof(CategoryName))]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Category name must have min length of 3 and max length of 100")]
        [Required(ErrorMessage = "Category name is required")] 
        public string CategoryName { get; set; }
        public Int64 MotherCategoryId { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Category Description must have min length of 3 and max length of 500")]
        public string Description { get; set; }
        public string? ImageUrl { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

       
    }
}