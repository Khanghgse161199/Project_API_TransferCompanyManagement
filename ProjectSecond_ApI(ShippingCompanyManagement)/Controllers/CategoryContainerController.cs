using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.CategoryContainerservices;
using Services.TransitCarServices;
using ViewModel.CategoryTran;
using ViewModel.TransitCar;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryContainerController : ControllerBase
    {
        private readonly ICategoryContainerservice _CategoryContainer;
        private readonly IAuthService _Auth;
        public CategoryContainerController(ICategoryContainerservice categoryContainerservice, IAuthService authService)
        {
            _CategoryContainer = categoryContainerservice;
            _Auth = authService;
        }

        [HttpPost("CreateCategoryContainer")]
        public async Task<IActionResult> CreateCategoryContainer(CreateCategoryContainerViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _CategoryContainer.CreateCategoryTranAsync(info.name);
                        if (isCreate)
                        {
                            return Ok("Create CategoryContainer Success");
                        }
                        else return BadRequest("Error when create CategoryContainer!");
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

        [HttpGet("GetAllOfCategoryContainer")]
        public async Task<IActionResult> GetAllOfCategoryContainer()
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
                            var isGets = await _CategoryContainer.GetAllCategoryContainersAsync();
                            if (isGets.Count > 0)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have CategoryContainer finded.");
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

        [HttpGet("GetCategoryContainerById")]
        public async Task<IActionResult> GetCategoryContainerById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _CategoryContainer.GetCategoryTranById(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have CategoryContainer finded.");
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

        [HttpPut("UpdateCategoryContainer")]
        public async Task<IActionResult> UpdateCategoryContainer(string id, UpdateCategoryContainerViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _CategoryContainer.UpdateCategoryTranAsync(id,info.name);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update CategoryContainer!");
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

        [HttpPut("DeleteCategoryContainer")]
        public async Task<IActionResult> DeleteCategoryContainer(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null && checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _CategoryContainer.DeleteCategoryTranAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete CategoryContainer");
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
