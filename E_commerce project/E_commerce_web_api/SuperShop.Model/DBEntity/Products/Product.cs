using SuperShop.Common.Enum;
using SuperShop.Model.DBEntity.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class Product : BaseEntity
    {
        public Product()
        {
            OrderDetails = new List<OrderDetails>();
        }

        [ForeignKey("Category")]
        public Int64? CategoryId { get; set; }

        [ForeignKey("Brand")]
        public Int64? BrandId { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must have min length of 3 and max length of 100")]
        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Product Description must have min length of 3 and max length of 500")]
        public string Description { get; set; }

        // If Select UnitType = CountableProducts, do nothing
        // If Select UnitType = LengthUnitType, load LengthUnitType on dropdown 
        // If Select UnitType = VolumeUnitType, load VolumeUnitType on dropdown
        // If Select UnitType = WeightUnitType, load WeightUnitType on dropdown
        public UnitType? UnitType { get; set; }

        // Only insert when LengthUnitType is seleced from UnitType 
        public LengthUnitType? LengthUnitType { get; set; }

        // Only insert when VolumeUnitType is seleced from UnitType 
        public VolumeUnitType? VolumeUnitType { get; set; }

        // Only insert when WeightUnitType is seleced from UnitType 
        public WeightUnitType? WeightUnitType { get; set; }

        public string? ImageUrl { get; set; }

        public string SKU { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Brand? Brand { get; set; }
        public virtual AddToCart? AddToCart { get; set; }
        public virtual ICollection<OrderDetails>? OrderDetails { get; set; }
        
    }
}
