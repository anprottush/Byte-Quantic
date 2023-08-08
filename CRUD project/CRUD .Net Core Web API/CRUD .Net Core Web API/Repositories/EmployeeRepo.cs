using CRUD_.Net_Core_Web_API.Interfaces;
using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Models.DB;

namespace CRUD_.Net_Core_Web_API.Repositories
{
    public class EmployeeRepo : IRepo<Employee, int, bool>
    {
        private readonly ProjectDbContext db;
        public EmployeeRepo(ProjectDbContext db)
        {
            this.db = db;
        }
        public bool Create(Employee employee)
        {
            if (employee != null)
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            var ext = db.Employees.Find(id);
            db.Employees.Remove(ext);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Employee Get(int id)
        {
            var employees = (from e in db.Employees
                               where e.Id == id
                               select e).SingleOrDefault();
            if (employees != null)
            {
                return employees;
            }
            else
            {
                return null;
            }
        }

        public ICollection<Employee> GetAll()
        {
            var employees = db.Employees.ToList();
            if (employees != null)
            {
                return employees;
            }
            else
            {
                return null;
            }
        }

        public bool Update(Employee employee)
        {
            var ext = db.Employees.Find(employee.Id);
            db.Entry(ext).CurrentValues.SetValues(employee);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
