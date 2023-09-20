using SuperShop.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class ProductRating : BaseEntity
    {
        public Int64 CustomerId { get; set; }

        public Int64 ProductId { get; set; }

        public decimal RatingValue { get; set; }

        public string Description { get; set; }

        public ProductRatingReson ProductRatingReson { get; set; }
    }
}
