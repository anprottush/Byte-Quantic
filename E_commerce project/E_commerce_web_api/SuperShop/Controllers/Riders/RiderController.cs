using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Services;
using SuperShop.Model.DBEntity.Rider;
using Microsoft.AspNetCore.Authorization;
using SuperShop.Services.Common;

namespace SuperShop.Controllers.Riders
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RiderController : ControllerBase
    {
        private readonly IBaseService<RiderInfo> _service;
        private readonly IBaseService<RiderAddress> _addressService;
        private readonly IBaseService<RiderRating> _ratingService;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.ERiderImagePath;

        public RiderController(IBaseService<RiderInfo> service, IBaseService<RiderRating> ratingService, IBaseService<RiderAddress> addressService, ICommonServices commonServices)
        {
            _service = service;
            _ratingService = ratingService;
            _addressService = addressService;
            _commonService = commonServices;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<RiderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
        {
            var response = new PayloadResponse<RiderInfo>
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

            RiderInfo data = await _service.GetByIdAsync(id);

            IEnumerable<RiderAddress> addresses = await _addressService.GetAsync(pa => pa.RiderId == id);

            data.RiderAddresses = new List<RiderAddress>(addresses);

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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<RiderInfo>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "RiderAddresses", pageNumber: pageNumber, pageSize: pageSize);

            foreach (var rider in response.Data)
            {
                rider.RiderAddresses = rider.RiderAddresses.ToList();
            }

            var data = response.Data.ToList();

            return Ok(new PayloadResponse<PaginatedResponse<RiderInfo>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<RiderInfo>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<RiderInfo>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }



        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Post(RiderInfo model)
        {
            OperationResult result = await _service.AddAsync(model, null);
            var response = new PayloadResponse<RiderInfo>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new RiderInfo(),
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
        [ProducesResponseType(typeof(PayloadResponse<RiderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, RiderInfo model)
        {
            var response = new PayloadResponse<RiderInfo>
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

            RiderInfo oldData = await _service.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.FirstName = model.FirstName;
            oldData.LastName = model.LastName;
            oldData.Email = model.Email;
            oldData.PhoneNo= model.PhoneNo;
            oldData.MobileNo= model.MobileNo;
            oldData.CityId = model.CityId;
            oldData.IsApproved = model.IsApproved;
            oldData.Description = model.Description;
            oldData.RiderNo = model.RiderNo;

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
        [ProducesResponseType(typeof(PayloadResponse<RiderInfo>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<RiderInfo>
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

            var riderResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = riderResponse.Success;
            response.message = new List<string>() { riderResponse.Message };
            response.payload = riderResponse.Result;

            if (!riderResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<RiderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> GetRatingById(long id)
        {
            var response = new PayloadResponse<RiderRating>
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

            RiderRating data = await _ratingService.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<RiderRating>>), 200)]
        [Route("ratings")]
        public async Task<IActionResult> GetAllRating(int pageNumber, int pageSize)
        {
            var response = await _ratingService.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<RiderRating>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<RiderRating>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<RiderRating>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }



        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("ratings")]
        public async Task<IActionResult> PostRating(RiderRating model)
        {
            OperationResult result = await _ratingService.AddAsync(model, null);
            var response = new PayloadResponse<RiderRating>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new RiderRating(),
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
        [ProducesResponseType(typeof(PayloadResponse<RiderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> UpdateRating(long id, RiderRating model)
        {
            var response = new PayloadResponse<RiderRating>
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

            RiderRating oldData = await _ratingService.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(PayloadResponse<RiderRating>), 200)]
        [Route("ratings/{id:int}")]
        public async Task<IActionResult> DeleteRating(long id)
        {
            var response = new PayloadResponse<RiderRating>
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

            var riderRatingresponse = await _ratingService.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = riderRatingresponse.Success;
            response.message = new List<string>() { riderRatingresponse.Message };
            response.payload = riderRatingresponse.Result;

            if (!riderRatingresponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<RiderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> GetaddressById(long id)
        {
            var response = new PayloadResponse<RiderAddress>
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

            RiderAddress data = await _addressService.GetByIdAsync(id);

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
        public async Task<IActionResult> AddAddresses(long id, [FromBody] List<RiderAddress> address)
        {
            var response = new PayloadResponse<List<RiderAddress>>
            {
                success = false,
                message = new List<string>() { "Please enter valid rider ID and address data." },
                payload = null,
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (id <= 0 || address == null || address.Count == 0)
            {
                return ErrorResponse.BadRequest(response);
            }

            RiderInfo rider = await _service.GetByIdAsync(id);

            if (rider == null)
            {
                response.message = new List<string>() { "Rider not found." };
                return NotFound(response);
            }

            OperationResult result = await _addressService.AddRangeAsync(address, null);

            if (result.Success)
            {
                return Created($"{response.request_url}", new PayloadResponse<List<RiderAddress>>
                {
                    success = true,
                    message = new List<string>() { ApiResponseMessage.Retrive },
                    payload = address,
                    operation_type = PayloadType.Save,
                    request_time = requestTime,
                    response_time = Utilities.GetRequestResponseTime()
                });
            }

            response.message = new List<string>() { "Failed to add address." };
            return BadRequest(response);
        }


        [HttpPut]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<RiderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> UpdateAddress(long id, RiderAddress model)
        {
            var response = new PayloadResponse<RiderAddress>
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

            RiderAddress oldData = await _addressService.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            oldData.RiderId = model.RiderId;
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
        [ProducesResponseType(typeof(PayloadResponse<RiderAddress>), 200)]
        [Route("address/{id:int}")]
        public async Task<IActionResult> DeleteAddress(long id)
        {
            var response = new PayloadResponse<RiderAddress>
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

            var riderAddressResponse = await _addressService.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = riderAddressResponse.Success;
            response.message = new List<string>() { riderAddressResponse.Message };
            response.payload = riderAddressResponse.Result;

            if (!riderAddressResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }


        [HttpPost("image/{rider_id}")]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<RiderInfo>), 200)]
        public async Task<IActionResult> UploadRiderImage(IFormFile imageFile, long rider_id)
        {
            if (rider_id <= 0)
            {
                return ErrorResponse.BadRequest(rider_id);
            }

            if (imageFile != null)
            {
                var riderInfo = await _service.GetByIdAsync(rider_id);

                if (riderInfo == null)
                {
                    return ErrorResponse.BadRequest($"Rider with ID {rider_id} not found.");
                }

                var uploadResult = await _commonService.UploadImageWebPFormat_Supershop(pictureFolder, "", formFile: imageFile);

                if (!uploadResult.success)
                {
                    return ErrorResponse.BadRequest(uploadResult.message.FirstOrDefault());
                }

               
                riderInfo.ImageUrl = uploadResult.data.FinalURL;
                await _service.Update(riderInfo, User.Identity.Name);

                return Ok(new PayloadResponse<RiderInfo>
                {
                    success = true,
                    message = new List<string> { ApiResponseMessage.Retrive },
                    payload = riderInfo,
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
        [ProducesResponseType(typeof(PayloadResponse<List<RiderInfo>>), 200)]
        [Route("image/{rider_id}")]
        public async Task<IActionResult> GetAllRiderPictures(long rider_id)
        {

            if (rider_id <= 0)
            {
                return ErrorResponse.BadRequest(rider_id);
            }

            var response = await _service.GetByIdAsync(rider_id);

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
