using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Model.CustomValidation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}
