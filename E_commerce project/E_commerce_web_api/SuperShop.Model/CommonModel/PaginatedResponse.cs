using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.CommonModel
{
    public class PaginatedResponse<TEntity> where TEntity : class
    {
        // The paginated list of data items
        public List<TEntity> Data { get; set; }

        // The total number of data items
        public int TotalCount { get; set; }
    }
}
