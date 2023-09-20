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
    public class Brand : BaseEntity
    {
        public Brand()
        {
            Products = new List<Product>();
        }

        [Unique(typeof(ApplicationDbContext), nameof(BrandName))]
        [StringLength(100, MinimumLength = 3, ErrorMessage ="Brand name must have min length of 3 and max length of 100")]
        [Required(ErrorMessage = "Brand name is required")]
        public string BrandName { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Brand Description must have min length of 3 and max length of 500")]
        public string Description { get; set; }
        public string? BaseUrl { get; set; }
        public string? SubFolderLocation { get; set; } // Image Sub Directory
        public string? ImageUrl { get; set; }

        public virtual ICollection<Product>? Products { get; set; }

       
    }
}