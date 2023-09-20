using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class ProductProvider : BaseEntity
    {
        [Required]
        public Int64 ProductId { get; set; }

        [Required]
        public Int64 ProviderId { get; set; }
    }
}
