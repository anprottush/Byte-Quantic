using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Model.DBEntity.Provider;

namespace SuperShop.Model.DBEntity.Customers
{
    public class AddToCart : BaseEntity
    {
        [ForeignKey("Customer")]
        public Int64 CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey("Product")]
        public Int64 ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("ProviderInfo")]
        public Int64? ProviderId { get; set; }
        public virtual ProviderInfo? ProviderInfo { get; set; }
    }
}
