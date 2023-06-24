using DataService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.TransitCarServices;
using ViewModel.TransitCar;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TransitCarController : ControllerBase
    {
        private readonly ITransitCarService _transitCarService;
        private readonly IAuthService _Auth;
        public TransitCarController(ITransitCarService transitCarService, IAuthService authService)
        {
            _transitCarService = transitCarService;
            _Auth = authService;
        }

        [HttpPost("CreateTransitCar")]
        public async Task<IActionResult> CreateTransitCar(CreateTransitCarViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
           
                if (!string.IsNullOrEmpty(tokenInHeader) && info != null) { 
                    var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                    if (checkToken != null && checkToken.RoleName == "Manager")
                    {
                        if (ModelState.IsValid)
                        {
                            bool isCreate = await _transitCarService.CreateTransitCarAsync(info.name, info.brand, DateTime.Now, DateTime.Now.AddYears(20), info.OriginCompany);
                            if (isCreate)
                            {
                                return Ok("Create TransitCar Success");
                            }
                            else return BadRequest("Error when create transit car!");
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

        [HttpGet("GetAllOfTransitCar")]
        public async Task<IActionResult> GetAllOfTransitCar()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if(checkToken.RoleName == "Manager" || checkToken.RoleName == "Employee")
                {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _transitCarService.GetAllTransitCarAsync();
                            if (isGets.Count != null)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have trasitCar finded.");
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

        [HttpGet("GetTransitCarById")]
        public async Task<IActionResult> GetTransitCarById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager" || checkToken.RoleName == "Employee")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _transitCarService.GetTransitCarById(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have trasitCar finded.");
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

        [HttpPut("UpdateTransitCar")]
        public async Task<IActionResult> UpdateTransitCar(string id,UpdateTransitCarViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _transitCarService.UpdateTransitCarAsync(id, info.name, info.brand, info.OriginCompany, info.registerDate, info.outOfDate);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update transit car!");
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

        [HttpPut("DeleteTransitCar")]
        public async Task<IActionResult> DeleteTransitCar(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _transitCarService.DeleteTransitCarAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete Transitcar");
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
