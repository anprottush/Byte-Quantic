using SuperShop.Model.DBEntity.Customers;
using SuperShop.Model.DBEntity.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Returns
{
    public class ReturnDetails : BaseEntity
    {
        [ForeignKey("Return")]
        public Int64 ReturnId { get; set; }
        public virtual Return? Return { get; set; }

        [ForeignKey("Product")]
        public Int64 ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public int Quantity { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey("OrderDetails")]
        public Int64 OrderDetailsId { get; set; }
        public virtual OrderDetails? OrderDetails { get; set; }
        public virtual ICollection<ReturnImage>? ReturnImages { get; set; }
    }
}
