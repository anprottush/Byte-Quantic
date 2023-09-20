using SuperShop.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Customers
{
    public class CustomerAddress : BaseEntity
    {
        [ForeignKey("Customer")]
        public Int64 CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string? PhoneNumber { get; set; }

        public Int64 CityAddress { get; set; }
    }
}
