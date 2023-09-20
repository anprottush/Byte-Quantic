using SuperShop.Common.Enum;
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
    public class Return : BaseEntity
    {
        public Return() 
        {
            ReturnDetails = new HashSet<ReturnDetails>();
        }
        public string ReturnNo { get; set; }

        [ForeignKey("Order")]
        public Int64? OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public bool IsFullReturn { get; set; }

        public decimal ReturnTotalAmount { get; set; }

        public string Reason { get; set; }

        public ReturnStatus Status { get; set; }

        public ReturnReason ReturnReason { get; set; }
        public virtual ICollection<ReturnDetails>? ReturnDetails { get; set; }
        public virtual ReturnImage? ReturnImage { get; set; }
    }
}
