using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common.Enum;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Provider;
using SuperShop.Services;
using Microsoft.AspNetCore.Authorization;

namespace SuperShop.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize]
	public class ProviderAddressController : ControllerBase
    {
        private readonly IBaseService<ProviderAddress> _service;
        private readonly string requestTime = Utilities.GetRequestResponseTime();


        public ProviderAddressController(IBaseService<ProviderAddress> addressService)
        {
            _service = addressService;

        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<ProviderAddress>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById(long id)
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

            ProviderAddress data = await _service.GetByIdAsync(id);

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
        [ProducesResponseType(typeof(PayloadResponse<PaginatedResponse<ProviderAddress>>), 200)]
        [Route("")]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize)
        {
            var response = await _service.GetPaginateAsync(filter: x => x.IsActive && !x.IsRemoved, orderBy: null, includeProperties: "", pageNumber: pageNumber, pageSize: pageSize);

            var data = response.Data.ToList();
            return Ok(new PayloadResponse<PaginatedResponse<ProviderAddress>>
            {
                success = response.Data != null ? true : false,
                message = new List<string>() { response.Data != null ? ApiResponseMessage.Retrive : ApiResponseMessage.Unsuccess },
                payload = response.Data != null ? new PaginatedResponse<ProviderAddress>() { Data = data, TotalCount = response.TotalCount } : new PaginatedResponse<ProviderAddress>(),
                operation_type = PayloadType.GetAllData,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            });
        }


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Post(ProviderAddress model)
        {
          
            OperationResult result = await _service.AddAsync(model, null);
            var response = new PayloadResponse<ProviderAddress>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new ProviderAddress(),
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
        [ProducesResponseType(typeof(PayloadResponse<ProviderAddress>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, ProviderAddress model)
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

            ProviderAddress oldData = await _service.GetByIdAsync(id);

            if (oldData == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);
            }

            OperationResult result = await _service.Update(model, User.Identity.Name);
            //oldData.ProviderId = model.ProviderId;
            oldData.AddressType= model.AddressType;
            oldData.IsDefault = model.IsDefault;
            oldData.AddressLine1 = model.AddressLine1;
            oldData.AddressLine2 = model.AddressLine2;
            oldData.Latitude = model.Latitude;
            oldData.Longitude = model.Longitude;
            oldData.PhoneNumber= model.PhoneNumber;
            oldData.CityAddress= model.CityAddress;

           

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
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
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

            var providerAddressResponse = await _service.UpdateIsRemoveTrue(id, User.Identity.Name);
            response.success = providerAddressResponse.Success;
            response.message = new List<string>() { providerAddressResponse.Message };
            response.payload = providerAddressResponse.Result;

            if (!providerAddressResponse.Success)
            {
                return ErrorResponse.BadRequest(id);
            }

            return Ok(response);
        }

    }
}
