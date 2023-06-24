using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.CategoryBlockServices;
using Services.CategoryContainerservices;
using ViewModel.CategoryBlock;
using ViewModel.CategoryTran;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryBlockController : ControllerBase
    {
        private readonly ICategoryBlockService _categoryBlock;
        private readonly IAuthService _Auth;
        public CategoryBlockController(ICategoryBlockService categoryBlockService, IAuthService authService)
        {
            _categoryBlock = categoryBlockService;
            _Auth = authService;
        }

        [HttpPost("CreateCategoryBlocks")]
        public async Task<IActionResult> CreateCategoryBlocks(CreateCategoryBlockViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _categoryBlock.CreateCategoryBlockAsync(info.name);
                        if (isCreate)
                        {
                            return Ok("Create CategoryBlock Success");
                        }
                        else return BadRequest("Error when create CategoryBlock!");
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

        [HttpGet("GetAllOfCategoryBlock")]
        public async Task<IActionResult> GetAllOfCategoryBlock()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager" || checkToken.RoleName == "Employee")
                {
                    if (checkToken != null)
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _categoryBlock.GetAllCategoryBlockAsync();
                            if (isGets.Count > 0)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have CategoryBlock finded.");
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

        [HttpGet("GetCategoryBlockById")]
        public async Task<IActionResult> GetCategoryBlockById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager" || checkToken.RoleName == "Employee")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _categoryBlock.GetCategoryBlockById(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have CategoryBlock finded.");
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

        [HttpPut("UpdateCategoryBlock")]
        public async Task<IActionResult> UpdateCategoryBlock(string id, UpdateCateogryBlockViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager")
                {
                    if (ModelState.IsValid)
                    {
                        var isUpdate = await _categoryBlock.UpdateCategoryBlockAsync(id, info.name);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update CategoryBlock!");
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

        [HttpPut("DeleteCategoryBlock")]
        public async Task<IActionResult> DeleteCategoryBlock(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if ((checkToken.RoleName == "Admin" || checkToken.RoleName == "Manager"))
                {
                    if (ModelState.IsValid)
                    {
                        var isDelete = await _categoryBlock.DeleteCategoryBlockAsync(id);
                        if (isDelete)
                        {
                            return Ok("Delete Success");
                        }
                        else return NotFound("Error When Delete CategoryBlock");
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
