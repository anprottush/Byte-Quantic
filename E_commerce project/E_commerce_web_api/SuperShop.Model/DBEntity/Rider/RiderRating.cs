using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Rider
{
    public class RiderRating : BaseEntity
    {
        [ForeignKey("RiderInfo")]
        public Int64 RiderId { get; set; }

        // Customer and Provider can rate rider after product delivery
        public Int64? CustomerId { get; set; }

        public Int64? ProviderId { get; set; }

        public Int64 OrderId { get; set; }

        public decimal RatingValue { get; set; }

        public string Description { get; set; }
        public virtual RiderInfo ?RiderInfo { get; set; }
    }
}
