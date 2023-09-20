using SuperShop.Model.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Provider
{
    public class ProviderInfo : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string MobileNo { get; set; }
        public string? ImageUrl { get; set; }

        public Int64 CityId { get; set; }

        public bool IsApproved { get; set; }

        public string Description { get; set; }

        public string ProviderNo { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<ProviderAddress>? ProviderAddresses { get; set; }
        public virtual ICollection<ProviderRating>? ProviderRatings { get; set; }

        public ProviderInfo()
        {
            ProviderAddresses = new List<ProviderAddress>();
            ProviderRatings = new List<ProviderRating>();
        }
    }
}
