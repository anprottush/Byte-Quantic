using SuperShop.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.SwaggerRequestExamples
{
    public class LoginModelExamples : IExamplesProvider<LoginModel>
    {
        public LoginModel GetExamples()
        {
            return new LoginModel
            {
                Username= "john.doe",
                Password="123456"
            };
        }
    }
}
