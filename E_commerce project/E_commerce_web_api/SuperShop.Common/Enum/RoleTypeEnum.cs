using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Common.Enum
{
    public enum RoleTypeEnum
    {
        // Application owner
        SuperAdmin,
        Admin,
        Manager,
        Operator,

        // Provider
        Provider,

        Rider,
        Customer
    }
}
