using SuperShop.Model.ViewModel;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.SwaggerResponseExamples
{
    public class LoginNotFoundResponseExamples : IExamplesProvider<LoginNotFoundResponseViewModel>
    {
        public LoginNotFoundResponseViewModel GetExamples()
        {
            return new LoginNotFoundResponseViewModel
            {
                successResonse = null,
                failedResponse = new FailedLoginResponse
                {
                    Error = 404,
                    Token = ""
                }
            };
        }
    }
}
