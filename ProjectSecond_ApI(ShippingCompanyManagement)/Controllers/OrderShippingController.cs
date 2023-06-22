using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.OrderDetailServices;
using Services.OrderSerivces;
using ViewModel.Order;
using ViewModel.OrderDetail;
using ViewModel.OrderShipping;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderShippingController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly IAuthService _Auth;
        public OrderShippingController(IOrderDetailService orderDetailService, IAuthService authService)
        {
            _orderDetailService = orderDetailService;
            _Auth = authService;
        }

        [HttpPost("CreateOrderShiping")]
        public async Task<IActionResult> CreateOrderShiping(CreateOrderShippingViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _orderDetailService.CreateOrderDetailAsync(info.OrderId,info.BlockId,info.WorkMappingId,info.Price);
                        if (isCreate)
                        {
                            return Ok("Create OrderShiping Success");
                        }
                        else return BadRequest("Error when create OrderShiping!");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpGet("GetAllOfOrderShipping")]
        public async Task<IActionResult> GetAllOfOrderShipping()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Manager")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _orderDetailService.GetAllOrderDetailAsync();
                            if (isGets.Count > 0)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have OrderShipping finded.");
                        }
                        else
                        {
                            var error = ModelState.Select(p => p.Value.Errors)
                                .Where(p => p.Count > 0)
                                .ToList();
                            return BadRequest(error);
                        }
                    }
                    else return NoContent();
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpGet("GetOrderShippingById")]
        public async Task<IActionResult> GetOrderShippingById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _orderDetailService.GetOrderDetailByIdAsync(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have OrderShipping finded.");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpGet("GetOrderShippingByMainOrder")]
        public async Task<IActionResult> GetOrderShippingByMainOrder(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _orderDetailService.GetOrederShippingByMainOrder(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have OrderShipping finded.");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpGet("GetOrderShippingByWorkingMapping")]
        public async Task<IActionResult> GetOrderShippingByWorkingMapping(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Manager" || checkToken.RoleName == "Driver")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _orderDetailService.GetOrderDetailByWorkingMappingAsync(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have OrderShipping finded.");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPut("UpdateOrderShipping")]
        public async Task<IActionResult> UpdateOrderShipping(string id, UpdateOrderShppingViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Employee" || checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _orderDetailService.UpdateOrderDetailAsync(id, info.OrderId,info.BlockId,info.WorkMappingId,info.Price);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update OrderShipping!");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPut("UpdateIsDoneOrderShipping")]
        public async Task<IActionResult> UpdateIsDoneOrderShipping(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Driver" || checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _orderDetailService.UpdateIsDoneAsync(id);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update Working OrderShipping!");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }

        [HttpPut("DeleteOrderShipping")]
        public async Task<IActionResult> DeleteOrderShipping(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _orderDetailService.DeleteOrderDetailAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete OrderShipping");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
                    }
                }
                else return NotFound();
            }
            else return NotFound();
        }
    }
}
