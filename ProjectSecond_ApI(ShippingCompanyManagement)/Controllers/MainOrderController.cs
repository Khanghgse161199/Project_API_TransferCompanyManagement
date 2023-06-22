using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.CategoryBlockServices;
using Services.OrderSerivces;
using ViewModel.CategoryBlock;
using ViewModel.Order;
using ViewModel.OrderShipping;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainOrderController : ControllerBase
    {
        private readonly IMainOrderService _mainOrderService;
        private readonly IAuthService _Auth;
        public MainOrderController(IMainOrderService mainOrderService, IAuthService authService)
        {
            _mainOrderService = mainOrderService;
            _Auth = authService;
        }

        [HttpPost("CreateMainOrder")]
        public async Task<IActionResult> CreateMainOrder(CreateMainOrderViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _mainOrderService.CreateMainOrderAsync(info.id,info.Reciver, info.ReciverPhone, info.ReciverEmail, info.ReciverAddress, info.Sender, info.ReciverCitizenId, info.Total);
                        if (isCreate)
                        {
                            return Ok("Create MainOrder Success");
                        }
                        else return BadRequest("Error when create MainOrder!");
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

        [HttpGet("GetAllOfMainOrder")]
        public async Task<IActionResult> GetAllOfMainOrder()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken != null && checkToken.RoleName == "Manager")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _mainOrderService.GetAllMainOrderAsync();
                            if (isGets.Count > 0)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have mainOrder finded.");
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

        [HttpGet("GetMainOrderById")]
        public async Task<IActionResult> GetMainOrderById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _mainOrderService.GetMainOrderById(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have MainOrder finded.");
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

        [HttpPut("UpdateMainOrder")]
        public async Task<IActionResult> UpdateMainOrder(string id, UpdateMainOrderViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _mainOrderService.UpdateMainOrderAsync(id, info.Reciver, info.ReciverPhone, info.ReciverEmail, info.ReciverAddress, info.Sender, info.ReciverCitizenId, info.Total);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update MainOrder!");
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

        [HttpPut("UpdateIsDoneMainOrder")]
        public async Task<IActionResult> UpdateIsDoneMainOrder(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _mainOrderService.UpdateIsDoneMainOrderAsync(id);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update MainOrder!");
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

        [HttpPut("DeleteMainWorking")]
        public async Task<IActionResult> DeleteMainWorking(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _mainOrderService.DeleteMainOrder(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete MainOrder");
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
