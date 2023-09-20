using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.ViewModel;
using SuperShop.Services.Common;
using SuperShop.Services;
using SuperShop.Model.DBEntity.Customers;
using DocumentFormat.OpenXml.Spreadsheet;
using SuperShop.Common.Enum;
using SuperShop.Model.DBEntity.Products;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace SuperShop.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IBaseService<Customer> _service;
        private readonly IBaseService<CustomerAddress> _serviceAddress;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.ECustomerProfileImagePath;
        public CustomerController(IBaseService<Customer> service, IBaseService<CustomerAddress> serviceAddress, ICommonServices commonService)
        {
            _service = service;
            _serviceAddress = serviceAddress;
            _commonService = commonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Customer>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<Customer>
            {
                success = false,
                message = new List<string>() { "Please enter valid Id." },
                payload = null,
                operation_type = PayloadType.GetById,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };
            if (id <= 0)
            {
                return ErrorResponse.BadRequest(id);
            }

            Customer customer = await _service.GetByIdAsync(id);

            if (customer == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);

            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = customer;
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<Customer>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<Customer>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<Customer>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<Customer>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });


        }

        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Customer model)
        {
            OperationResult result = await _service.AddAsync(model, User.Identity.Name);
            var response = new PayloadResponse<Customer>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new Customer(),
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (result.Success)
            {
                return Created($"{response.request_url}", response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Customer>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, Customer model)
        {
            var response = new PayloadResponse<Customer>
            {
                success = false,
                message = new List<string>() { "" },
                payload = null,
                operation_type = PayloadType.Update,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };
            if (id <= 0)
            {
                response.message = new List<string>() { "Enter valid Id" };
                return ErrorResponse.BadRequest(response);
            }

            Customer oldCustomer = await _service.GetByIdAsync(id);

            if (oldCustomer == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            OperationResult result = await _service.Update(model, User.Identity.Name);

            if (result.Success)
            {
                response.success = result.Success;
                response.message = new List<string>() { result.Message };
                response.payload = result.Result;
                return Ok(response);
            }

            response.message = new List<string>() { result.Message };
            response.payload = model;
            return BadRequest(response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Customer>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<Customer>
            {
                success = false,
                message = new List<string>() { "Please enter valid Id." },
                payload = null,
                operation_type = PayloadType.GetById,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (id == 0)
            {
                return ErrorResponse.BadRequest(response);
            }

            var brandResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = brandResponse.Success;
            response.message = new List<string>() { brandResponse.Message };
            response.payload = brandResponse.Result;

            if (!brandResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("{customer_id}/images/{name}/name")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Customer>), 200)]
        public async Task<IActionResult> UploadProfilePictures([FromForm(Name = "files")] List<IFormFile> formFiles, long customer_id, string name = "")
        {
            bool successFlag = true;
            string errorMessage = "";

            if (customer_id <= 0)
            {
                return ErrorResponse.BadRequest(customer_id);
            }

            if (formFiles != null && formFiles.Count > 0)
            {
                var response = await _service.GetByIdAsync(customer_id);
                if (response == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statuscode = 404,
                        message = "Customer id not found",
                    });
                }
                
                else
                {
                    foreach (var file in formFiles.Select((value, index) => new { index, value }))
                    {
                        if (file.value.ContentType == "image/jpeg" || file.value.ContentType == "image/jpg" || file.value.ContentType == "image/png")
                        {
                            var result = await _commonService.UploadImageWebPFormat_Supershop(fileFolder: pictureFolder, "", formFile: file.value);

                            if (!result.success)
                            {
                                successFlag = false;
                                errorMessage = result.message.FirstOrDefault();
                                break;
                            }

                            if (file.index == 0)
                            {
                                
                                response.ProfileImage = result.data.FinalURL;

                                var customerResponse = await _service.Update(response, User.Identity.Name);
                            }

                            await _service.AddAsync(new Customer()
                            {
                                FirstName = name,
                                ProfileImage = result.data.FinalURL,
                            }, User.Identity.Name);
                        }
                        else
                        {
                            successFlag = false;
                            errorMessage = "Incorrect File Format. Only Accept image/jpeg, image/jpg, image/png";
                        }
                    }
                }


                if (!successFlag)
                {
                    return ErrorResponse.BadRequest(errorMessage);
                }

                var data = new Customer()
                {
                    Id = response.Id,
                    FirstName = response.FirstName,
                    LastName = response.LastName,
                    Email = response.Email,
                    CountryCode = response.CountryCode,
                    MobileNumber = response.MobileNumber,
                    PhoneNumber = response.PhoneNumber,
                    CustomerLevel = response.CustomerLevel,
                    ProfileImage = response.ProfileImage,
                };

                return Ok(new PayloadResponse<Customer>
                {
                    success = data != null ? true : false,
                    message = new List<string>() { data != null ? ApiResponseMessage.Save_success : ApiResponseMessage.Unsuccess },
                    payload = data != null ? data : new Customer(),
                    operation_type = PayloadType.Save,
                    request_time = requestTime,
                    response_time = Utilities.GetRequestResponseTime()
                });
            }
            else
            {
                return new BadRequestObjectResult(new
                {
                    errors = new
                    {
                        formFile = new[] { formFiles == null ? "Please select a file" : "Invalid file" },
                    },
                    type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    traceId = HttpContext != null ? HttpContext.TraceIdentifier : ""
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Customer>), 200)]
        [Route("{customer_id}/images")]
        public async Task<IActionResult> GetAllCustomerPictures(long customer_id)
        {
            if (customer_id <= 0)
            {
                return ErrorResponse.BadRequest(customer_id);
            }

            var response = await _service.GetByIdAsync(customer_id);

            if (response == null)
            {
                return NotFound();
            }

            var data = new Customer()
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Email = response.Email,
                CountryCode = response.CountryCode,
                MobileNumber = response.MobileNumber,
                PhoneNumber = response.PhoneNumber,
                CustomerLevel = response.CustomerLevel,
                ProfileImage = response.ProfileImage,
            };

            return Ok(new PayloadResponse<Customer>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? data : null,
                operation_type = PayloadType.GetById,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }

    }
}
