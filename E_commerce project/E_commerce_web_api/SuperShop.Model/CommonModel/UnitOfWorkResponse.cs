using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Model.CommonModel
{
    public class UnitOfWorkResponse
    {
        public List<string> message { get; set; }
        public bool success { get; set; }

        public static UnitOfWorkResponse Error(string message = null)
        {
            return new UnitOfWorkResponse
            {
                message = new List<string> { message ?? "There was a problem handling the request." },
                success = false
            };
        }

        public static UnitOfWorkResponse Success(string message = null)
        {
            return new UnitOfWorkResponse
            {
                message = new List<string> { message ?? "Request successful." },
                success = true
            };
        }
    }
}
