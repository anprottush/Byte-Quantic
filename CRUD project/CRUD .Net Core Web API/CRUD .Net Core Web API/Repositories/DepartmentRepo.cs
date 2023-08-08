using CRUD_.Net_Core_Web_API.Interfaces;
using CRUD_.Net_Core_Web_API.Models;
using CRUD_.Net_Core_Web_API.Models.DB;

namespace CRUD_.Net_Core_Web_API.Repositories
{
    public class DepartmentRepo : IRepo<Department, int, bool>
    {
        private readonly ProjectDbContext db;
        public DepartmentRepo(ProjectDbContext db)
        {
            this.db = db;
        }
        public bool Create(Department department)
        {
            if (department != null)
            {
                db.Departments.Add(department);
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
            var ext = db.Departments.Find(id);
            db.Departments.Remove(ext);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Department Get(int id)
        {
            var departments = (from d in db.Departments
                               where d.Id == id
                               select d).SingleOrDefault();
            if (departments != null)
            {
                return departments;
            }
            else
            {
                return null;
            }
        }

        public ICollection<Department> GetAll()
        {
            var departments = db.Departments.ToList();
            if (departments != null)
            {
                return departments;
            }
            else
            {
                return null;
            }
        }

        public bool Update(Department department)
        {
            var ext = db.Departments.Find(department.Id);
            db.Entry(ext).CurrentValues.SetValues(department);
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
