using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Rider
{
    public class RiderInfo : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }

        public Int64 CityId { get; set; }

        public string ?ImageUrl { get; set; }

        public bool IsApproved { get; set; }

        public string Description { get; set; }

        public string RiderNo { get; set; }

        public virtual ICollection<RiderAddress>? RiderAddresses { get; set; }
        public virtual ICollection<RiderRating>? RiderRatings { get; set; }

        public RiderInfo()
        {
            RiderAddresses = new List<RiderAddress>();
            RiderRatings = new List<RiderRating>();
        }
    }
}
