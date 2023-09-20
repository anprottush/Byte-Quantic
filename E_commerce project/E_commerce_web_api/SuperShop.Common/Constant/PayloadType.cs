using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Common.Constant
{
    public static class PayloadType
    {
        public const string Login = "Login";
        public const string Logout = "Logout";
        public const string Registration = "Registration";
        public const string ForgetPassword = "Forget Password";

        public const string GetAllData = "Get All Data";
        public const string GetById = "Get Data By Id";
        public const string GetByCondition = "Get Data By Condition";

        public const string Save = "Data Save";
        public const string Update = "Data Update";
        public const string Delete = "Data Delete";

        public const string ActiveInactive = "Active Inactive";
    }
}
