using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common;
using SuperShop.Common.Constant;
using SuperShop.Common.Enum;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Model.ViewModel;
using SuperShop.Services;
using SuperShop.Services.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BrandController : ControllerBase
    {
        private readonly IBaseService<Brand> _service;
        private readonly IBaseService<Product> _serviceProduct;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.EBrandImagePath;
        public BrandController(IBaseService<Brand> service, IBaseService<Product> productService, ICommonServices commonService)
        {
            _service = service;
            _serviceProduct = productService;
            _commonService = commonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Brand>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<Brand>
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

            Brand brand = await _service.GetByIdAsync(id);

            if (brand == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
                
            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = brand;
            return Ok(response);
           
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<Brand>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<Brand>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<Brand>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<Brand>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });

        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Brand model)
        {
            OperationResult result = await _service.AddAsync(model, User.Identity.Name);
            var response = new PayloadResponse<Brand>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new Brand(),
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
        [ProducesResponseType(typeof(PayloadResponse<Brand>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, Brand model)
        {
            var response = new PayloadResponse<Brand>
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

            Brand oldBrand = await _service.GetByIdAsync(id);

            if (oldBrand == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }
            oldBrand.Id = id;
            oldBrand.BrandName = model.BrandName;
            oldBrand.Description = model.Description;
            

            OperationResult result = await _service.Update(oldBrand, User.Identity.Name);

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
        [ProducesResponseType(typeof(PayloadResponse<Brand>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<Brand>
            {
                success = false,
                message = new List<string>() { "Please enter valid Id." },
                payload = null,
                operation_type = null,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (id == 0)
            {
                return ErrorResponse.BadRequest(response);
            }

            var brandresponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = brandresponse.Success;
            response.message = new List<string>() { brandresponse != null  ? ApiResponseMessage.Delete_success : ApiResponseMessage.Unsuccess };
            response.payload = brandresponse.Result;
            response.operation_type = PayloadType.Delete;
            if (!brandresponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("images/{brand_id:int}")] 
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Brand>), 200)]
        public async Task<IActionResult> UploadBrandPictures([FromForm(Name = "files")] List<IFormFile> formFiles, long brand_id)
        {
            var response = new PayloadResponse<Brand>
            {
                success = false,
                message = new List<string>() { "" },
                payload = null,
                operation_type = null,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (brand_id <= 0)
            {
                response.success = false;
                response.message = new List<string>() { "Enter valid Id" };
                return ErrorResponse.BadRequest(response);
                
            }
            
            if (formFiles != null && formFiles.Count > 0)
            {
                var brandresponse = await _service.GetByIdAsync(brand_id);
               
                if (brandresponse == null)
                {
                    response.success = false;
                    response.message = new List<string>() { "Brand id not found" };
                    return NotFound(response);
                }
                
                else
                {
                    foreach (var file in formFiles.Select((value, index) => new { index, value }))
                    {
                        var uploadresult = await _commonService.UploadImageWebPFormat_Supershop(fileFolder: pictureFolder, "", formFile: file.value);
                        
                        if (!uploadresult.success)
                        {
                            return ErrorResponse.BadRequest(uploadresult.message.FirstOrDefault());
                        }
                        if (file.index == 0)
                        {                           
                            brandresponse.BaseUrl = uploadresult.data.BaseURL;
                            brandresponse.SubFolderLocation = uploadresult.data.SubFolderLocation;
                            brandresponse.ImageUrl = uploadresult.data.FinalURL;

                            OperationResult result = await _service.Update(brandresponse, User.Identity.Name);

                            response.success = result != null ? true : false;
                            response.message = new List<string>() { result != null ? "Image Upload Successfully" : ApiResponseMessage.Unsuccess };
                            response.payload = result != null ? brandresponse : new Brand();
                            response.operation_type = PayloadType.Save;
                        }                       
                    }

                    return Ok(response);
                
            }
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
        [ProducesResponseType(typeof(PayloadResponse<Brand>), 200)]
        [Route("{brand_id}/images")]
        public async Task<IActionResult> GetAllBrandPictures(long brand_id)
        {
           
            if (brand_id <= 0)
            {
                return ErrorResponse.BadRequest(brand_id);
            }

            var response = await _service.GetByIdAsync(brand_id);

            if (response == null)
            {
                return NotFound();
            }

            var data = new Brand()
            {
                Id = response.Id,
                BrandName = response.BrandName,
                Description = response.Description,
                ImageUrl = response.ImageUrl
            };

            return Ok(new PayloadResponse<Brand>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? data : null,
                operation_type = PayloadType.GetById,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<Product>>), 200)]
        [Route("products")]
        public async Task<IActionResult> GetByProducts([FromQuery] List<long> brandId)
        {
         
            if (brandId == null || brandId.Count == 0)
            {
                return ErrorResponse.BadRequest("Brand id required.");
            }

            var response = await _serviceProduct.GetAsync(
                filter: b => brandId.Contains(b.BrandId ?? 0) && b.IsActive && !b.IsRemoved,
                orderBy: null,
                includeProperties: "");
            if (response == null)
            {
                return NotFound();
            }

            var data = response.Select(p => new Product()
            {
                ProductName = p.ProductName,
                CategoryId = p.CategoryId,
                Description = p.Description,
                Id = p.Id,
                BrandId = p.BrandId,
                UnitType = p.UnitType,
                LengthUnitType = p.LengthUnitType,
                VolumeUnitType = p.VolumeUnitType,
                WeightUnitType = p.WeightUnitType,
                ImageUrl = p.ImageUrl,
                SKU = p.SKU,
                Brand = p.Brand

            }).ToList();

            return Ok(new PayloadResponse<List<Product>>
            {
                success = response != null ? true : false,
                message = new List<string>() { response != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response != null ? data : new List<Product>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }
    }
}
