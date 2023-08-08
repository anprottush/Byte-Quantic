namespace CRUD_.Net_Core_Web_API.Models
{
    public class Department
    {
        public Department()
        {
            this.Employees = new HashSet<Employee>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Employee> Employees { get; set; } 

    }
}
