using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services.Common;
using SuperShop.Services;
using SuperShop.Model.DBEntity.Customers;
using Microsoft.AspNetCore.Authorization;

namespace SuperShop.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
	//[Authorize]
	public class AddToCartController : ControllerBase
    {
        private readonly IBaseService<AddToCart> _service;
        private readonly IBaseService<Customer> _serviceCustomer;
        private readonly IBaseService<Product> _serviceProduct;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;

        public AddToCartController(IBaseService<AddToCart> service, IBaseService<Customer> serviceCustomer, IBaseService<Product> serviceProduct, ICommonServices commonService)
        {
            _service = service;
            _serviceCustomer = serviceCustomer;
            _serviceProduct = serviceProduct;
            _commonService = commonService;

        }

        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<Product>>), 200)]
        [Route("id")]
        public async Task<IActionResult> GetById(long customerId)
        {
            var response = new PayloadResponse<List<Product>>
            {
                success = false,
                message = new List<string>() { "Please enter valid Id." },
                payload = null,
                operation_type = PayloadType.GetById,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };
            if (customerId <= 0)
            {
                return ErrorResponse.BadRequest(customerId);
            }

            var product = await _serviceProduct.GetAsync(ci => ci.Id == customerId);
            
            if (product == null)
            {
                response.success = false;
                response.message = new List<string>() { ApiResponseMessage.NotFound };
                return NotFound(response);

            }
            response.success = true;
            response.message = new List<string>() { ApiResponseMessage.Retrive };
            response.payload = product.ToList();
            return Ok(response);
        }
        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddToCart model)
        {
            var response = new PayloadResponse<AddToCart>
            {
                success = false,
                message = new List<string>() { "" },
                payload = null,
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };
            var customer = await _serviceCustomer.GetByIdAsync(model.CustomerId);
            var product = await _serviceProduct.GetByIdAsync(model.ProductId);
            OperationResult result =null; 
            var cartItem = new AddToCart
            {
                Customer = customer,
                Product = product,
                Quantity = model.Quantity
            };
            if(cartItem!=null)
            {
                result = await _service.AddAsync(cartItem, User.Identity.Name);
                
            }

            
           

            if (result.Success)
            {
                return Created($"{response.request_url}", response);
            }

            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<AddToCart>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(long id, AddToCart model)
        {
            var response = new PayloadResponse<AddToCart>
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

            var oldCart = await _service.GetByIdAsync(id);

            if (oldCart == null)
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
        [ProducesResponseType(typeof(PayloadResponse<AddToCart>), 200)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(long id)
        {
            var response = new PayloadResponse<AddToCart>
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
    }
}
