using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Provider;
using SuperShop.Services;
using Microsoft.AspNetCore.Authorization;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services.Common;

namespace SuperShop.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize]
	public class ProviderController : ControllerBase
    {
        private readonly IBaseService<ProviderInfo> _service;
        private readonly IBaseService<ProviderAddress> _addressService;
        private readonly IBaseService<ProviderRating> _ratingService;
        private readonly IBaseService<Product> _productService;
        private readonly IBaseService<ProductProvider> _productProviderService;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.EProviderImagePath;

        public ProviderController(IBaseService<ProviderInfo> service,IBaseService<ProviderRating> ratingservice, IBaseService<ProviderAddress> addressService, IBaseService<Product> productService, IBaseService<ProductProvider> productProviderService, ICommonServices commonServices)
        {
            _service = service;
            _ratingService = ratingservice;
            _addressService = addressService;
            _productService = productService;
            _productProviderService = productProviderService;
            _commonService = commonServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<ProviderInfo>
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
                return ErrorResponse.BadRequest(response);
            }

            ProviderInfo data = await _service.GetByIdAsync(id);
            

            IEnumerable<ProviderAddress> addresses = await _addressService.GetAsync(pa => pa.ProviderId == id);

            data.ProviderAddresses = new List<ProviderAddress>(addresses);

            if (data == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            

            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = data;
            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<ProviderInfo>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "ProviderAddresses", pageNumber: pageNumber, pageSize: pageSize);

            foreach (var provider in response.Data)
            {
                provider.ProviderAddresses = provider.ProviderAddresses.ToList();
            }
            var data = response.Data.ToList();
            
            return Ok(new PayloadResponse<PaginatedResponse<ProviderInfo>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<ProviderInfo>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<ProviderInfo>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Post(ProviderInfo model)
        {
            OperationResult result = await _service.AddAsync(model, null);
            var response = new PayloadResponse<ProviderInfo>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new ProviderInfo(),
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
        [ProducesResponseType(typeof(PayloadResponse<ProviderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, ProviderInfo model)
        {
            var response = new PayloadResponse<ProviderInfo>
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

            ProviderInfo oldData = await _service.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.CompanyName = model.CompanyName;
            oldData.Email = model.Email;
            oldData.PhoneNo = model.PhoneNo;
            oldData.MobileNo= model.MobileNo;
            oldData.CityId = model.CityId;
            oldData.IsApproved= model.IsApproved;
            oldData.Description = model.Description;
            oldData.ProviderNo = model.ProviderNo;

            OperationResult result = await _service.Update(oldData, null);

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
        [ProducesResponseType(typeof(PayloadResponse<ProviderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<ProviderInfo>
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

            var providerResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = providerResponse.Success;
            response.message = new List<string>() { providerResponse.Message };
            response.payload = providerResponse.Result;

            if (!providerResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            var response = new PayloadResponse<ProviderRating>
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
                return ErrorResponse.BadRequest(response);
            }

            ProviderRating data = await _ratingService.GetByIdAsync(id);

            if (data == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = data;
            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<ProviderRating>>), 200)]
        [Route("ratings")]
        public async Task<IActionResult> GetAllRating(int pageNumber, int pageSize)
        {
            var response = await _ratingService.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<ProviderRating>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<ProviderRating>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<ProviderRating>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }



        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("ratings")]
        public async Task<IActionResult> PostRating(ProviderRating model)
        {
            OperationResult result = await _ratingService.AddAsync(model, null);
            var response = new PayloadResponse<ProviderRating>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new ProviderRating(),
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
        [ProducesResponseType(typeof(PayloadResponse<ProviderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> UpdateRating(long id, ProviderRating model)
        {
            var response = new PayloadResponse<ProviderRating>
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

            ProviderRating oldData = await _ratingService.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.RiderId = model.RiderId;
            oldData.CustomerId = model.CustomerId;
            oldData.ProviderId = model.ProviderId;
            oldData.OrderId = model.OrderId;
            oldData.RatingValue = model.RatingValue;
            oldData.Description = model.Description;

            OperationResult result = await _ratingService.Update(oldData, null);

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
        [ProducesResponseType(typeof(PayloadResponse<ProviderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            var response = new PayloadResponse<ProviderRating>
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

            var providerRatingresponse = await _ratingService.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = providerRatingresponse.Success;
            response.message = new List<string>() { providerRatingresponse.Message };
            response.payload = providerRatingresponse.Result;

            if (!providerRatingresponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> GetaddressById(long id)
        {
            var response = new PayloadResponse<ProviderAddress>
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
                return ErrorResponse.BadRequest(response);
            }

            ProviderAddress data = await _addressService.GetByIdAsync(id);

            if (data == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = data;
            return Ok(response);
        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> AddAddresses(long id, [FromBody] List<ProviderAddress> address)
        {
            var response = new PayloadResponse<List<ProviderAddress>>
            {
                success = false,
                message = new List<string>() { "Please enter valid provider ID and address data." },
                payload = null,
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (id <= 0 || address == null || address.Count == 0)
            {
                return ErrorResponse.BadRequest(response);
            }

            ProviderInfo provider = await _service.GetByIdAsync(id);

            if (provider == null)
            {
                response.message = new List<string>() { "Provider not found." };
                return NotFound(response);
            }

            OperationResult result = await _addressService.AddRangeAsync(address, null);

            if (result.Success)
            {
                return Created($"{response.request_url}", new PayloadResponse<List<ProviderAddress>>
                {
                    success = true,
                    message = new List<string>() { ApiResponseMessage.Retrive },
                    payload = address,
                    operation_type = PayloadType.Save,
                    request_time = requestTime,
                    response_time = Utilities.GetRequestResponseTime()
                });
            }

            response.message = new List<string>() { "Failed to add addresses." };
            return BadRequest(response);
        }


        [HttpPut]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> UpdateAddress(long id, ProviderAddress model)
        {
            var response = new PayloadResponse<ProviderAddress>
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

            ProviderAddress oldData = await _addressService.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.ProviderId = model.ProviderId;
            oldData.AddressType = model.AddressType;
            oldData.IsDefault = model.IsDefault;
            oldData.AddressLine1 = model.AddressLine1;
            oldData.AddressLine2 = model.AddressLine2;
            oldData.Latitude = model.Latitude;
            oldData.Longitude = model.Longitude;
            oldData.PhoneNumber = model.PhoneNumber;
            oldData.CityAddress = model.CityAddress;

            OperationResult result = await _addressService.Update(oldData, User.Identity.Name);

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
        [ProducesResponseType(typeof(PayloadResponse<ProviderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var response = new PayloadResponse<ProviderAddress>
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

            var providerAddressResponse = await _addressService.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = providerAddressResponse.Success;
            response.message = new List<string>() { providerAddressResponse.Message };
            response.payload = providerAddressResponse.Result;

            if (!providerAddressResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [Authorize(Roles = "Provider")]
        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("products")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var providerInfo = _service.Get(x => x.ApplicationUser.Id == Convert.ToInt32(User.Identity.Name)).FirstOrDefault();

            if (providerInfo.Id == null)
            {
                return BadRequest("Invalid Id");
            }

            var id = providerInfo.Id;

            if (product == null)
            {
                var response = new PayloadResponse<Product>
                {
                    success = false,
                    message = new List<string>() { "Invalid product data or Provider Id" },
                    payload = null,
                    operation_type = PayloadType.Save,
                    request_time = requestTime,
                    response_time = Utilities.GetRequestResponseTime()

                };

                return BadRequest(response);
            }

            bool productExists = await _productService.AnyAsync(p => p.ProductName.ToLower() == product.ProductName.ToLower());


            if (productExists)
            {
                var response = new PayloadResponse<Product>
                {
                    success = false,
                    message = new List<string>() { "Product with the same name already exists." },
                    payload = null,
                    operation_type = PayloadType.Save,
                    request_time = requestTime,
                    response_time = Utilities.GetRequestResponseTime()
                };

                return BadRequest(response);
            }

            OperationResult productResult = await _productService.AddAsync(product, User.Identity.Name);

            if (productResult.Success)
            {
                var productProvider = new ProductProvider
                {
                    ProviderId = id,
                    ProductId = product.Id
                };

                OperationResult productProviderResult = await _productProviderService.AddAsync(productProvider, User.Identity.Name);

                if (productProviderResult.Success)
                {
                    var response = new PayloadResponse<Product>
                    {
                        success = true,
                        message = new List<string>() { "Product added successfully." },
                        payload = product,
                        operation_type = PayloadType.Save,
                        request_time = requestTime,
                        response_time = Utilities.GetRequestResponseTime()
                    };

                    return Created($"{response.request_url}", response);
                }
            }

            var errorResponse = new PayloadResponse<Product>
            {
                success = false,
                message = new List<string>() { "Failed to add the product." },
                payload = null,
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            return BadRequest(errorResponse);
        }


        [HttpPost("image/{provider_id}")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderInfo>), 200)]
        public async Task<IActionResult> UploadRiderImage(IFormFile imageFile, long provider_id)
        {
            if (provider_id <= 0)
            {
                return ErrorResponse.BadRequest(provider_id);
            }

            if (imageFile != null)
            {
                var providerInfo = await _service.GetByIdAsync(provider_id);

                if (providerInfo == null)
                {
                    return ErrorResponse.BadRequest($"Rider with ID {provider_id} not found.");
                }

                var uploadResult = await _commonService.UploadImageWebPFormat_Supershop(pictureFolder, "", formFile: imageFile);

                if (!uploadResult.success)
                {
                    return ErrorResponse.BadRequest(uploadResult.message.FirstOrDefault());
                }


                providerInfo.ImageUrl = uploadResult.data.FinalURL;
                await _service.Update(providerInfo, User.Identity.Name);

                return Ok(new PayloadResponse<ProviderInfo>
                {
                    success = true,
                    message = new List<string> { ApiResponseMessage.Retrive },
                    payload = providerInfo,
                    operation_type = PayloadType.Update,
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
                        imageFile = new[] { "Please select a file" },
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
        [ProducesResponseType(typeof(PayloadResponse<List<ProviderInfo>>), 200)]
        [Route("image/{provider_id}")]
        public async Task<IActionResult> GetAllRiderPictures(long provider_id)
        {

            if (provider_id <= 0)
            {
                return ErrorResponse.BadRequest(provider_id);
            }

            var response = await _service.GetByIdAsync(provider_id);

            if (response == null || string.IsNullOrEmpty(response.ImageUrl))
            {
                return NotFound();
            }

            return Ok(new PayloadResponse<string>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.ImageUrl,
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }
    }
}
