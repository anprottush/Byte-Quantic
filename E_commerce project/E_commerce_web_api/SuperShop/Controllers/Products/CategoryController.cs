using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common;
using SuperShop.Common.Constant;
using SuperShop.Common.Enum;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services;
using SuperShop.Services.Common;

namespace SuperShop.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IBaseService<Category> _service;
        private readonly IBaseService<Product> _productService;
        private readonly IBaseService<CategoryImage> _serviceImage;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.ECategoryImagePath;

        public CategoryController(IBaseService<Category> service, IBaseService<Product> productService, IBaseService<CategoryImage> imageService, ICommonServices commonService)
        {
            _service = service;
            _productService = productService;
            _serviceImage = imageService;
            _commonService = commonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Category>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<Category>
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

            Category data = await _service.GetByIdAsync(id);
            
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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<Category>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<Category>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<Category>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<Category>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }

        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Post(Category model)
        {
            OperationResult result = await _service.AddAsync(model, null);
            var response = new PayloadResponse<Category>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new Category(),
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
        [ProducesResponseType(typeof(PayloadResponse<Category>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, Category model)
        {
            var response = new PayloadResponse<Category>
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

            Category oldData = await _service.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.CategoryName = model.CategoryName;
            oldData.Description = model.Description;
            oldData.MotherCategoryId = model.MotherCategoryId;
       
            OperationResult result = await _service.Update(oldData, User.Identity.Name);

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
        [ProducesResponseType(typeof(PayloadResponse<Category>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<Category>
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

            var categoryResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = categoryResponse.Success;
            response.message = new List<string>() { categoryResponse.Message };
            response.payload = categoryResponse.Result;

            if (!categoryResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpPost]
        [Route("{category_id}/images/{description}/description")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<CategoryImage>>), 200)]
        public async Task<IActionResult> UploadCategoryPictures([FromForm(Name = "files")] List<IFormFile> formFiles, long category_id, string description = "")
        {
            if (category_id <= 0)
            {
                return ErrorResponse.BadRequest(category_id);
            }

            if (formFiles != null && formFiles.Count > 0)
            {
                var response = await _service.GetByIdAsync(category_id);

                var imageResponse = await _serviceImage.GetAsync(x => x.Category.Id == response.Id);

                if (formFiles.Count > 0)
                {
                    foreach (var file in formFiles.Select((value, index) => new { index, value }))
                    {
                        var uploadResult = await _commonService.UploadImageWebPFormat_Supershop(pictureFolder, "", formFile: file.value);

                        if (!uploadResult.success)
                        {
                            return ErrorResponse.BadRequest(uploadResult.message.FirstOrDefault());
                        }

                        if (file.index == 0)
                        {
                            response.ImageUrl = uploadResult.data.FinalURL;
                            _service.Update(response, User.Identity.Name);

                            var categoryResponse = _service.Update(response, User.Identity.Name);
                        }

                        await _serviceImage.AddAsync (new CategoryImage()
                        {
                            Category = response,
                            Description = description,
                            BaseUrl = uploadResult.data.BaseURL,
                            SubFolderLocation = uploadResult.data.SubFolderLocation,
                            FileName = uploadResult.data.FileName,
                            FinalUrl = uploadResult.data.FinalURL
                        }, User.Identity.Name);
                    }
                }

                var categoryImages = imageResponse.Select(x => new CategoryImage
                {
                    Category = response,
                    Id = x.Id,
                    Description = x.Description,
                    BaseUrl = x.BaseUrl,
                    SubFolderLocation = x.SubFolderLocation,
                    FileName = x.FileName,
                    FinalUrl = x.FinalUrl
                }).ToList();

                return Ok(new PayloadResponse<List<CategoryImage>>
                {
                    success = categoryImages != null,
                    message = new List<string> { categoryImages != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                    payload = categoryImages ?? new List<CategoryImage>(),
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
        [ProducesResponseType(typeof(PayloadResponse<List<CategoryImage>>), 200)]
        [Route("{category_id}/images")]
        public async Task<IActionResult> GetAllCategoryPictures(long category_id)
        {
            //Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy = (query) =>
            //{
            //    return query.OrderByDescending(category => category.Id);
            //};
            if (category_id <= 0)
            {
                return ErrorResponse.BadRequest(category_id);
            }

            var response = await _serviceImage.GetAsync(filter: x => x.Category.Id == category_id && x.IsActive && !x.IsRemoved);

            return Ok(new PayloadResponse<List<CategoryImage>>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? response.ToList() : new List<CategoryImage>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<Product>>), 200)]
        [Route("products")]
        public async Task<IActionResult> GetProductsByCategoryIds([FromQuery] List<long> ListOfId)
        {
            if (ListOfId == null || ListOfId.Count == 0)
            {
                return ErrorResponse.BadRequest("Category ID are required.");
            }

            var response = await _productService.GetAsync(
                filter: p => ListOfId.Contains(p.CategoryId ?? 0) && p.IsActive && !p.IsRemoved,
                orderBy: null,
                includeProperties: "");

            var data = response.Select(p => new Product()
            {
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Id = p.Id,
                BrandId= p.BrandId,
                UnitType = p.UnitType,
                LengthUnitType = p.LengthUnitType,
                VolumeUnitType= p.VolumeUnitType,
                WeightUnitType=p.WeightUnitType,
                ImageUrl = p.ImageUrl,
                SKU = p.SKU
                
            }).ToList();

            return Ok(new PayloadResponse<List<Product>>
            {
                success = response != null,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? data : new List<Product>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }
    }
}
