using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.CategoryContainerservices;
using Services.ContainerServices;
using ViewModel.CategoryTran;
using ViewModel.Container;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {
        private readonly IContainerService _containerService;
        private readonly IAuthService _Auth;
        public ContainerController(IContainerService containerService, IAuthService authService)
        {
            _containerService = containerService;
            _Auth = authService;
        }
       
        [HttpPost("CreateContainer")]
        public async Task<IActionResult> CreateContainer(CreateContainerViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _containerService.CreateContainerAysnc(info.name, info.CategoryTransId, info.Weight);
                        if (isCreate)
                        {
                            return Ok("Create Container Success");
                        }
                        else return BadRequest("Error when create Container!");
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

        [HttpGet("GetAllOfContainer")]
        public async Task<IActionResult> GetAllOfContainer()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken != null)
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _containerService.GetAllContainerAsync();
                            if (isGets.Count != null)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have Container finded.");
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

        [HttpGet("GetContainerById")]
        public async Task<IActionResult> GetContainerById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _containerService.GetContainerByIdAsync(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have Container finded.");
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

        [HttpPut("UpdateContainer")]
        public async Task<IActionResult> UpdateContainer(string id, UpdateContainerViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _containerService.UpdateContainerAsync(id, info.name, info.Weight);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update Container!");
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

        [HttpPut("DeleteContainer")]
        public async Task<IActionResult> DeleteContainer(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _containerService.DeleteContainerAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete Container");
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
