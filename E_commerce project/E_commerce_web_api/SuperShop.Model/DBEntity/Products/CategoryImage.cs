using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.DBEntity.Products
{
    public class CategoryImage : BaseEntity
    {
        public virtual Category? Category { get; set; }
        public string? BaseUrl { get; set; }
        public string? SubFolderLocation { get; set; } // Image Sub Directory
        public string? FileName { get; set; } // Image Sub Directory
        public string? FinalUrl { get; set; }
        public string Description { get; set; }

        //public ICollection<Category> Categories { get; set; }
    }
}
