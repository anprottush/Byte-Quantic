using SuperShop.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Rider
{
    public class RiderAddress : BaseEntity
    {
        [ForeignKey("RiderInfo")]
        public Int64 RiderId { get; set; }

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string PhoneNumber { get; set; }

        public Int64 CityAddress { get; set; }

        public virtual RiderInfo ? RiderInfo { get; set; }
        
    }
}
