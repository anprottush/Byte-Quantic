using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SuperShop.Model.DBEntity
{
    public class RefreshToken : BaseEntity
    {
        [Required(ErrorMessage = "This field is required")]
        public string IdentityUserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Token { get; set; }
    }
}
