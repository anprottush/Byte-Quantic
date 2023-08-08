using System.ComponentModel.DataAnnotations;

namespace CRUD_.Net_Core_Web_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "User Name is required")]
        //[StringLength(10, ErrorMessage = "User Name should not exceeed 10 chars")]
        public string Username { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Password is required"), MinLength(8)]
        //[RegularExpression("(?=[A-Za-z0-9@#$%^&+!=]+$)^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+!=])(?=.{8,}).*$", ErrorMessage = "Invalid password must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
        public string Password { get; set; } = string.Empty;
        
    }
}
