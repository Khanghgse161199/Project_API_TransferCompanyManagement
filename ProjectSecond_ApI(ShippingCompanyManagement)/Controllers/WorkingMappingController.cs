using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.WorkingMappingSevices;
using ViewModel.WorkMapping;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingMappingController : ControllerBase
    {
        private readonly IWorkingMappingService _workingMappingService;
        private readonly IAuthService _Auth;
        public WorkingMappingController(IWorkingMappingService workingMappingService, IAuthService authService)
        {
            _workingMappingService = workingMappingService;
            _Auth = authService;
        }

        [HttpPost("CreateWorkingMapping")]
        public async Task<IActionResult> CreateWorkingMapping(CreateWorkMappingViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _workingMappingService.CreateWorkingMappingAsync(null, info.TransitCarId, info.ContainerId);
                        if (isCreate)
                        {
                            return Ok("Create WorkingMapping Success");
                        }
                        else return BadRequest("Error when create WorkingMapping!");
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

        [HttpGet("GetAllOfWorkingMapping")]
        public async Task<IActionResult> GetAllOfWorkingMapping()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _workingMappingService.GetAllWorkMappingAsync();
                            if (isGets.Count != null)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have WorkingMapping finded.");
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


        [HttpGet("GetWorkingMappingById")]
        public async Task<IActionResult> GetWorkingMappingById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _workingMappingService.GetWorkingMappingByIdAsync(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have WorkingMapping finded.");
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

        [HttpGet("GetAllOfWorkingMappingToRegiter")]
        public async Task<IActionResult> GetAllOfWorkingMappingToRegiter()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Driver" || checkToken.RoleName == "Manager" || checkToken.RoleName == "Admin")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _workingMappingService.GetAllWorkMappingToRegisterAsync();
                            if (isGets.Count != null)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have WorkingMapping finded.");
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

        [HttpGet("GetAllOfWorkingMappingEmployee")]
        public async Task<IActionResult> GetAllOfWorkingMappingEmployee(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName == "Manager" || checkToken.RoleName == "Driver")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _workingMappingService.GetAllWorkingMappingOfEmployeeAsync(id);
                            if (isGets.Count != null)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have WorkingMapping finded.");
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

        [HttpPut("RegiterWorkingMappingForEmployee")]
        public async Task<IActionResult> RegiterWorkingMappingForEmployee(string id, UpdateWorkMappingViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _workingMappingService.registerEmployeeWorkingMappingAsync(id, info.EmployeeId);
                        if (isUpdate)
                        {
                            return Ok("Register Success");
                        }
                        else return NotFound("Error when update WorkingMapping!");
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

        [HttpPut("UpdateStatusWorkingMapping")]
        public async Task<IActionResult> UpdateStatusWorkingMapping(string id, status status)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _workingMappingService.UpdateStatusWorkingMappingAsync(id, status);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update WorkingMapping!");
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

        [HttpPut("DeleteWorkingMapping")]
        public async Task<IActionResult> DeleteWorkingMapping(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _workingMappingService.DeletWorkingMappingAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete WorkingMapping");
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
