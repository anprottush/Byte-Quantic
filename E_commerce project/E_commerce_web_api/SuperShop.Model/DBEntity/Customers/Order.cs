using SuperShop.Common.Enum;
using SuperShop.Model.DBEntity.Provider;
using SuperShop.Model.DBEntity.Returns;
using SuperShop.Model.DBEntity.Rider;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Customers
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Returns = new HashSet<Return>();
        }

        [ForeignKey("Customer")]
        public Int64 CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey("CustomerAddress")]
        public Int64? CustomerAddressId { get; set; }
        public virtual CustomerAddress? CustomerAddress { get; set; }

        public string OrderNo { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public bool PaymentStatus { get; set; }

        [ForeignKey("ProviderInfo")]
        public Int64? ProviderId { get; set; }
        public virtual ProviderInfo? ProviderInfo { get; set; }

        [ForeignKey("RiderInfo")]
        public Int64? RiderId { get; set; }
        public virtual RiderInfo? RiderInfo { get; set; }
        // Check full delivery or partial delivary
        public bool IsFullDelivery { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public bool IsPartialPayment { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual ICollection<OrderDetails>? OrderDetails { get; set; }
        
        public virtual ICollection<Return>? Returns { get; set; }
        

    }
}
