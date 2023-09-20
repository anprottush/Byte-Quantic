using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Provider
{
    public class ProviderRating : BaseEntity
    {
        public Int64 ProviderId { get; set; }

        // Customer and Rider can rate Provider after product delivery
        public Int64? CustomerId { get; set; }

        public Int64? RiderId { get; set; }

        public Int64 OrderId { get; set; }

        public decimal RatingValue { get; set; }

        public string Description { get; set; }

        public virtual ProviderInfo ? ProviderInfo { get; set; }
    }
}
