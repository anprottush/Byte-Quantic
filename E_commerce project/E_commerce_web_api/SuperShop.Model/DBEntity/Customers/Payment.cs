using SuperShop.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Customers
{
    public class Payment : BaseEntity
    {
        [ForeignKey("Order")]
        public Int64? OrderId { get; set; }
        public virtual Order? Order { get; set; }

        [ForeignKey("Customer")]
        public Int64? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public PaymentType PaymentType { get; set; }

        public decimal GrossAmount { get; set; }

        public decimal Tax { get; set; }

        public DiscountType DiscountType { get; set; }

        public float Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public bool Status { get; set; }
    }
}
