using SuperShop.Common.Enum;
using SuperShop.Model.DBEntity.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Customers
{
    public class OrderDetails : BaseEntity
    {
        [ForeignKey("Order")]
        public Int64 OrderId { get; set; }
        public virtual Order? Order { get; set; }

        [ForeignKey("Product")]
        public Int64? ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int OrderQuantity { get; set; }

        public decimal UnitPrice { get; set; }

        public DiscountType DiscountType { get; set; }

        public decimal Discount { get; set; }

        public decimal GrossTotal { get; set; }

        public decimal SubTotal { get; set; }

        public bool IsDelivery { get; set; }

        public int DeliveryQuantity { get; set; }
    }
}
