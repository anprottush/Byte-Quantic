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
    public class ReturnImage : BaseEntity
    {
        public string? ImageUrl { get; set; }
        
        [ForeignKey("ReturnDetails")]
        public Int64? ReturnDetailsId { get; set; }
        public virtual ReturnDetails? ReturnDetails { get; set; }
    }
}
