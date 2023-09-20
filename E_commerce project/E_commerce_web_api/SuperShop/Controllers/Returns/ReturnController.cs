using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services.Common;
using SuperShop.Services;
using SuperShop.Model.DBEntity.Returns;
using SuperShop.Model.Migrations;

namespace SuperShop.Controllers.Returns
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReturnController : ControllerBase
    {
        private readonly IBaseService<Return> _service;
        private readonly IBaseService<ReturnImage> _serviceImage; 

        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.EReturnImagePath;
        public ReturnController(IBaseService<Return> service, IBaseService<ReturnImage> imageService, ICommonServices commonService)
        {
            _service = service;
            _serviceImage = imageService;
            _commonService = commonService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<Return>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<Return>
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

            Return returns = await _service.GetByIdAsync(id);

            if (returns == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);

            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = returns;
            return Ok(response);

        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<Return>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<Return>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<Return>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<Return>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });

        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Return model)
        {
            OperationResult result = await _service.AddAsync(model, User.Identity.Name);
            var response = new PayloadResponse<Return>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new Return(),
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
        [ProducesResponseType(typeof(PayloadResponse<Return>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, Return model)
        {
            var response = new PayloadResponse<Return>
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

            Return oldReturn = await _service.GetByIdAsync(id);

            if (oldReturn == null)
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
        [ProducesResponseType(typeof(PayloadResponse<Return>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<Return>
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

            var returnResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = returnResponse.Success;
            response.message = new List<string>() { returnResponse.Message };
            response.payload = returnResponse.Result;

            if (!returnResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("{return_id}/images/{description}/description")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ReturnImage>), 200)]
        public async Task<IActionResult> UploadReturnPictures([FromForm(Name = "files")] List<IFormFile> formFiles, long return_id, string description = "")
        {
            bool successFlag = true;
            string errorMessage = "";

            if (return_id <= 0)
            {
                return ErrorResponse.BadRequest(return_id);
            }

            if (formFiles != null && formFiles.Count > 0)
            {
                var response = await _service.GetByIdAsync(return_id);
                
                if (response == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        statuscode = StatusCode(404),
                        message = "Return id not found",
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
                                
                                response.ReturnImage.ImageUrl = result.data.FinalURL;

                                var brandResponse = _service.Update(response, User.Identity.Name);
                            }

                            await _serviceImage.AddAsync(new ReturnImage()
                            {
                               

                               ImageUrl = result.data.FinalURL
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

                var data = new ReturnImage()
                {
                    
                    ImageUrl = response.ReturnImage.ImageUrl
                };


                return Ok(new PayloadResponse<ReturnImage>
                {
                    success = data != null ? true : false,
                    message = new List<string>() { data != null ? ApiResponseMessage.Save_success : ApiResponseMessage.Unsuccess },
                    payload = data != null ? data : new ReturnImage(),
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
        [ProducesResponseType(typeof(PayloadResponse<ReturnImage>), 200)]
        [Route("{return_id}/images")]
        public async Task<IActionResult> GetAllBrandPictures(long return_id)
        {

            if (return_id <= 0)
            {
                return ErrorResponse.BadRequest(return_id);
            }

            var response = await _service.GetByIdAsync(return_id);

            if (response == null)
            {
                return NotFound();
            }

            var data = new ReturnImage()
            {
                
                ImageUrl = response.ReturnImage.ImageUrl
            };

            return Ok(new PayloadResponse<ReturnImage>
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
