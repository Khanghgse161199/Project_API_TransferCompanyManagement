using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthServices;
using Services.BlockServices;
using Services.CategoryBlockServices;
using ViewModel.Block;
using ViewModel.CategoryBlock;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly IBlockServices _blockServices;
        private readonly IAuthService _Auth;
        public BlockController(IBlockServices blockServices, IAuthService authService)
        {
            _blockServices = blockServices;
            _Auth = authService;
        }

        [HttpPost("CreateBlock")]
        public async Task<IActionResult> CreateBlock(CreateBlockViewModel info)
        {
            string tokenInHeader = Request.Headers["token"].ToString();

            if (!string.IsNullOrEmpty(tokenInHeader) && info != null)
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName != "Manager" && checkToken.RoleName == "Emloyee")
                {
                    if (ModelState.IsValid)
                    {
                        bool isCreate = await _blockServices.CreateBlockAsync(info.Name, info.Weight, info.CategoryBlockId);
                        if (isCreate)
                        {
                            return Ok("Create Block Success");
                        }
                        else return BadRequest("Error when create Block!");
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

        [HttpGet("GetAllOfBlocks")]
        public async Task<IActionResult> GetAllOfBlocks()
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    if (checkToken.RoleName != "Manager" && checkToken.RoleName == "Emloyee")
                    {
                        if (ModelState.IsValid)
                        {
                            var isGets = await _blockServices.GetAllBlockAsync();
                            if (isGets.Count > 0)
                            {
                                return Ok(isGets);
                            }
                            else return NotFound("Not have block finded.");
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

        [HttpGet("GetBlockById")]
        public async Task<IActionResult> GetBlockById(string id)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName != "Manager" && checkToken.RoleName == "Emloyee")
                {
                    if (ModelState.IsValid)
                    {
                        var isGet = await _blockServices.GetBlockByIdAsync(id);
                        if (isGet != null)
                        {
                            return Ok(isGet);
                        }
                        else return NotFound("Not have Block finded.");
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

        [HttpPut("UpdateBlock")]
        public async Task<IActionResult> UpdateCategoryBlock(string id, UpdateBlockViewModel info)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && info != null && !string.IsNullOrEmpty(id))
            {
                var checkToken = await _Auth.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName != "Manager" && checkToken.RoleName == "Emloyee")
                {
                    if (ModelState.IsValid) 
                    {
                        var isUpdate = await _blockServices.UpdateBlockAsync(id, info.Name, info.Weight, info.CategoryBlockId);
                        if (isUpdate)
                        {
                            return Ok("Update Success");
                        }
                        else return NotFound("Error when update Block!");
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
