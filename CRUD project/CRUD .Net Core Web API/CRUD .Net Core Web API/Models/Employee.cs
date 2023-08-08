using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_.Net_Core_Web_API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Employee Name is required")]
        //[StringLength(10, ErrorMessage = "Employee Name should not exceeed 10 chars")]
        public string Name { get; set; } = string.Empty;
        public string JoiningDate { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public double Salary { get; set; }
        

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
    }
}
