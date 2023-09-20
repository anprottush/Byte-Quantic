using SuperShop.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Customers
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();

            Orders = new HashSet<Order>();
        }

        [Required]
        [StringLength(50, MinimumLength =2)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string CountryCode { get; set; }

        public string? MobileNumber { get; set; }

        public string PhoneNumber { get; set; }

        public CustomerLevel CustomerLevel { get; set; }

        public string? ProfileImage { get; set; }
        public virtual AddToCart? AddToCart { get; set; }
        public virtual ICollection<CustomerAddress>? CustomerAddresses { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
       
    }
}