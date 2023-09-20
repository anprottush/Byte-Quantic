using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Common.Constant
{
    public static class ApiResponseMessage
    {
        public const string Save_success = "Success! Your data have been saved.";
        public const string Update_success = "Success! Your data have been updated.";
        public const string Delete_success = "Success! Your data have been deleted.";
        public const string Unsuccess = "Error: An unexpected problem occurred. Please try again later.";

        public const string Retrive = "Success! Data Retrieved Successfully.";
        public const string NotFound = "Data Not found. Please try again later.";
    }
}
