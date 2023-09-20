using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.ViewModel
{
    public class CategoryImageViewModel
    {
        public long id { get; set; }
        public long  category_id { get; set; }
        public string image_url { get; set; }
        public string description { get; set; }
    }
}
