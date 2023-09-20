using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Common.Constant;
using SuperShop.Common;
using SuperShop.Model.CommonModel;
using SuperShop.Model.DBEntity.Customers;
using SuperShop.Model.DBEntity.Products;
using SuperShop.Services.Common;
using SuperShop.Services;
using Microsoft.AspNetCore.Authorization;



namespace SuperShop.Controllers.Customers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IBaseService<Order> _service;
        private readonly IBaseService<Customer> _serviceCustomer;
        private readonly IBaseService<Product> _serviceProduct;
        private readonly IBaseService<AddToCart> _serviceAddToCart;
        private readonly string requestTime = Utilities.GetRequestResponseTime();
        private readonly ICommonServices _commonService;
        private readonly string pictureFolder = Utilities.ECustomerProfileImagePath;
        public OrderController(IBaseService<Order> service, IBaseService<Customer> serviceCustomer, IBaseService<Product> serviceProduct, IBaseService<AddToCart> serviceAddToCart, ICommonServices commonService)
        {
            _service = service;
            _serviceCustomer = serviceCustomer;
            _serviceProduct = serviceProduct;
            _serviceAddToCart = serviceAddToCart;
            _commonService = commonService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(PayloadResponse<List<Order>>), 200)]
        [Route("id")]
        public async Task<IActionResult> GetById(long customerId)
        {
            var response = new PayloadResponse<List<Order>>
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

            var product = await _service.GetAsync(ci => ci.Id == customerId);

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
       /* [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] Order model)
        {
            OperationResult result = await _service.AddAsync(model, User.Identity.Name);
            var response = new PayloadResponse<Order>
            {
                success = result.Success,
                message = new List<string>() { result.Message },
                payload = result.Result != null ? model : new Order(),
                operation_type = PayloadType.Save,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
            };

            if (result.Success)
            {
                return Created($"{response.request_url}", response);
            }

            return BadRequest(response);
        }*/


        [HttpPost]
        [ProducesResponseType(typeof(BadRequestObjectResult), 400)]
        [ProducesResponseType(typeof(CreatedResult), 201)]
        [Route("placeOrder")]
        public async Task<IActionResult> OrderPlace(int customerId)
        {
            var response = new PayloadResponse<Order>
            {
                success = false,
                message = new List<string>() { "" },
                payload = null,
                operation_type = null,
                request_time = requestTime,
                response_time = Utilities.GetRequestResponseTime()
   
            };

            if (customerId <= 0)
            {
                return ErrorResponse.BadRequest(customerId);
            }

            var customer = await _serviceCustomer.GetByIdAsync(customerId);
            var cartItems = _serviceAddToCart.Where(ci => ci.CustomerId == customerId).ToList();

            if (customer == null)
            {
                response.success = false;
                response.message = new List<string>() { "Customer not found." };
                response.operation_type = "Not Found";
                return NotFound(response);

            }
            if (cartItems.Count == 0)
            {
                return ErrorResponse.BadRequest("Cart is empty.");
            }
            var order = new Order()
            {
                CustomerId = customerId,
                Customer = customer,
                CustomerAddressId = 1,
                OrderNo = Guid.NewGuid().ToString(),
                DeliveryStatus = Common.Enum.DeliveryStatus.Pending,
                PaymentStatus = false,
                ProviderId = 1,
                RiderId = 1,
                IsFullDelivery = true,
                DeliveryDateTime = DateTime.Now,
                IsPartialPayment = false,
                //OrderDate = DateTime.Now
            };

            foreach (var cartItem in cartItems)
            {
                var orderDetails = new OrderDetails
                {
                    OrderId = order.Id,
                    Order = order,
                    ProductId = cartItem.ProductId,
                    Product = cartItem.Product,
                    OrderQuantity = cartItem.Quantity,
                    //UnitPrice = cartItem.Product.Price
                    DiscountType = Common.Enum.DiscountType.Percent,
                    Discount = 10,
                    GrossTotal = 100,
                    SubTotal = 200,
                    IsDelivery = true,
                    DeliveryQuantity = cartItem.Quantity,
                };
                order.OrderDetails.Add(orderDetails);
            }

            /*var payment = new Payment 
            {
                OrderId = model.Id,
                Order = order,
                CustomerId = model.CustomerId,
                Customer = customer,
               // Amount = inputModel.PaymentAmount, 
                
            };
            order.Payment = payment;*/
            OperationResult result = await _service.AddAsync(order, User.Identity.Name);


            // Remove cart items after placing order
            //_serviceAddToCart.RemoveRange(cartItems);

           

            if (result.Success)
            {
                response.success = true;
                response.message = new List<string>() { "Order placed successfully." };
                response.payload = result.Result != null ? order : new Order();
                response.operation_type = PayloadType.Save;
                return Created($"{response.request_url}", response);
            }

            return BadRequest(response);

           
        }

    }
}
