using SuperShop.Model.Data;
using SuperShop.Model.DBEntity.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Repository.TypeRepository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
    }
    public interface ICategoryRepository : IGenericRepository<Category> 
    {

    }
}
