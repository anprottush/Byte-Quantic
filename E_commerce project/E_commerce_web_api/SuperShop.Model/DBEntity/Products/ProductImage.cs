using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class ProductImage : BaseEntity
    {
        public ProductImage()
        {
            Products = new List<Product>();
        }
        public virtual Product? Product { get; set; }

        public string? BaseUrl { get; set; }
        public string? SubFolderLocation { get; set; } // Image Sub Directory
        public string? FileName { get; set; } // Image Sub Directory
        public string? FinalUrl { get; set; }
        public string Description { get; set; }

        public ICollection<Product>? Products { get; set; }

       
    }
}
