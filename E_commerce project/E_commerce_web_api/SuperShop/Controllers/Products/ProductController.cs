using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services;
using SuperShop.Services.Common;
using Microsoft.AspNetCore.Authorization;

namespace SuperShop.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IBaseService<Product> _service;
        private readonly IBaseService<ProductRating> _ratingService;
        private readonly IBaseService<ProductImage> _serviceImage;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.EProductImagePath;

        public ProductController(IBaseService<Product> service, IBaseService<ProductRating> ratingService, IBaseService<ProductImage> imageService, ICommonServices commonService)
        {
            _service = service;
            _ratingService = ratingService;
            _serviceImage = imageService;
            _commonService = commonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Product>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<Product>
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

            Product data = await _service.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<Product>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<Product>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<Product>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<Product>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Post(Product model)
        {
            OperationResult result = await _service.AddAsync(model, null);
            var response = new PayloadResponse<Product>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : null,
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
        [ProducesResponseType(typeof(PayloadResponse<Product>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, Product model)
        {
            var response = new PayloadResponse<Product>
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

            Product oldData = await _service.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            oldData.CategoryId = model.CategoryId;
            oldData.BrandId = model.BrandId;
            oldData.ProductName = model.ProductName;
            oldData.Description = model.Description;
            oldData.UnitType = model.UnitType;
            oldData.LengthUnitType = model.LengthUnitType;
            oldData.VolumeUnitType = model.VolumeUnitType;
            oldData.WeightUnitType = model.WeightUnitType;
            oldData.SKU= model.SKU;

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
        [ProducesResponseType(typeof(PayloadResponse<Product>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<Product>
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

            var productResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = productResponse.Success;
            response.message = new List<string>() { productResponse.Message };
            response.payload = productResponse.Result;

            if (!productResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("{product_id}/images/{description}/description")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<ProductImage>>), 200)]
        public async Task<IActionResult> UploadProductPictures([FromForm(Name = "files")] List<IFormFile> formFiles, long product_id, string description = "")
        {
            if (product_id <= 0)
            {
                return ErrorResponse.BadRequest(product_id);
            }

            if (formFiles != null && formFiles.Count > 0)
            {
                var response = await _service.GetByIdAsync(product_id);

                var imageResponse = await _serviceImage.GetAsync(x => x.Product.Id == response.Id);

                
                if (formFiles.Count > 0)
                {
                    foreach (var file in formFiles)
                    {
                        var uploadResult = await _commonService.UploadImageWebPFormat_Supershop(pictureFolder, "", formFile: file);

                        if (!uploadResult.success)
                        {
                            return ErrorResponse.BadRequest(uploadResult.message.FirstOrDefault());
                        }

                        if (response.ImageUrl == null)
                        {
                            response.ImageUrl = uploadResult.data.FinalURL;
                            _service.Update(response, User.Identity.Name);
                        }

                        await _serviceImage.AddAsync(new ProductImage()
                        {
                            Product = response,
                            Description = description,
                            BaseUrl = uploadResult.data.BaseURL,
                            SubFolderLocation = uploadResult.data.SubFolderLocation,
                            FileName = uploadResult.data.FileName,
                            FinalUrl = uploadResult.data.FinalURL
                        }, User.Identity.Name);
                    }
                }

                var productImages = imageResponse.Select(x => new ProductImage
                {
                    Product = response,
                    Description = x.Description,
                    BaseUrl = x.BaseUrl,
                    SubFolderLocation = x.SubFolderLocation,
                    FileName = x.FileName,
                    FinalUrl = x.FinalUrl
                }).ToList();

                return Ok(new PayloadResponse<List<ProductImage>>
                {
                    success = productImages != null,
                    message = new List<string> { productImages != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                    payload = productImages ?? new List<ProductImage>(),
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
        [ProducesResponseType(typeof(PayloadResponse<List<ProductImage>>), 200)]
        [Route("{product_id}/images")]
        public async Task<IActionResult> GetAllProductPictures(long product_id)
        {

            if (product_id <= 0)
            {
                return ErrorResponse.BadRequest(product_id);
            }

            var response = await _serviceImage.GetAsync(filter: x => x.Product.Id == product_id && x.IsActive && !x.IsRemoved);

            return Ok(new PayloadResponse<List<ProductImage>>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? response.ToList() : new List<ProductImage>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }



        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProductRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            var response = new PayloadResponse<ProductRating>
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

            ProductRating data = await _ratingService.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<ProductRating>>), 200)]
        [Route("ratings")]
        public async Task<IActionResult> GetAllRatings(int pageNumber, int pageSize)
        {
            var response = await _ratingService.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<ProductRating>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<ProductRating>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<ProductRating>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("ratings")]
        public async Task<IActionResult> PostRating(ProductRating model)
        {
            OperationResult result = await _ratingService.AddAsync(model, null);
            var response = new PayloadResponse<ProductRating>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new ProductRating(),
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
        [ProducesResponseType(typeof(PayloadResponse<ProductRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> UpdateRating(long id, ProductRating model)
        {
            var response = new PayloadResponse<ProductRating>
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

            ProductRating oldData = await _ratingService.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.ProductId= model.ProductId;
            oldData.CustomerId= model.CustomerId;
            oldData.RatingValue=model.RatingValue;
            oldData.Description = model.Description;
            oldData.ProductRatingReson= model.ProductRatingReson;


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
        [ProducesResponseType(typeof(PayloadResponse<ProductRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            var response = new PayloadResponse<ProductRating>
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

            var productResponse = await _ratingService.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = productResponse.Success;
            response.message = new List<string>() { productResponse.Message };
            response.payload = productResponse.Result;

            if (!productResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

    }
}
