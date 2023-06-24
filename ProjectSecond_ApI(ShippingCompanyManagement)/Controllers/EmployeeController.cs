using DataService.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AccountServices;
using Services.AuthServices;
using Services.EmployeeServices;
using Services.TokenServices;
using ViewModel.EmployeeViewModel;
using VireModel.EmployeeViewModel;

namespace ProjectSecond_ApI_ShippingCompanyManagement_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAccountService _accountRepository;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public EmployeeController(IEmployeeService employeeService, IAccountService accountService, IAuthService authService, ITokenService tokenService) {
            _employeeService = employeeService;
            _accountRepository = accountService;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel createEmployeeViewModel)
        {
            var tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin")
                {
                    if (ModelState.IsValid)
                    {
                        var accId = Guid.NewGuid().ToString();
                        var isCreateAcc = await _accountRepository.CreateAccountAsync(createEmployeeViewModel.Username, createEmployeeViewModel.Password, createEmployeeViewModel.RoleName, accId);
                        if (isCreateAcc)
                        {
                            var mailToken = Guid.NewGuid().ToString();
                            var isCreate = await _employeeService.CreateEmployeeAsync(createEmployeeViewModel, accId, mailToken);
                            if (isCreate)
                            {
                                var isSend = await _tokenService.SendMailConfirmAsync(createEmployeeViewModel.Email, mailToken);
                                if (isSend)
                                {
                                    return Ok("Check mail in 15min");
                                }
                                else return BadRequest();
                            }
                            else return BadRequest("Can't create Employee!");
                        }
                        else return BadRequest("Can't create Account!");
                    }
                    else
                    {
                        var error = ModelState.Select(p => p.Value.Errors)
                            .Where(p => p.Count > 0)
                            .ToList();
                        return BadRequest(error);
            }
                }
                else return BadRequest("Null Data");
            }
            else return NotFound("Not Have Token To Check");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginEmployee(LoginEmployeeViewModel loginEmployeeModel)
        {
            if (loginEmployeeModel != null)
            {
                var isLogin = await _authService.CheckLoginAsync(loginEmployeeModel);
                if (isLogin != null)
                {
                    return Ok(isLogin.AccessToken);
                }
                else return BadRequest("Error In Check Login");
            }
            else return NotFound();
        }

        [HttpPost("ReSendMailConfirm")]
        public async Task<IActionResult> ReSendMailConfirm(EmailEmployeeViewModel emailEmployeeViewModel)
        {
            if (emailEmployeeViewModel != null)
            {
                if (ModelState.IsValid)
                {
                    var isReSend = await _tokenService.ReSendMailConfirmAsync(emailEmployeeViewModel.Email);
                    if (isReSend)
                    {
                        return Ok("Check Your Mail in 15 min");
                    }
                    else return BadRequest();
                }
                else
                {
                    var error = ModelState.Select(p => p.Value.Errors)
                              .Where(p => p.Count > 0)
                              .ToList();
                    return BadRequest(error);
                }
            }else return NotFound();
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin")
                {
                    var allEmployee = await _employeeService.GetAllEmployeeAsync();
                    return Ok(allEmployee);
                }
                else return BadRequest();
            }
            else return BadRequest();
        }

        [HttpGet("GetEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(string Id)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id))
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin")
                {
                    var Employee = await _employeeService.GetEmployeeByIdAsync(Id);
                    return Ok(Employee);
                }
                else return BadRequest();
            }
            else return BadRequest();
        }

        [HttpGet("GetProfileEmployee")]
        public async Task<IActionResult> GetProfileEmployee()
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader))
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);           
                if(checkToken != null && checkToken.RoleName != "Admin")
                {
                    var profileEmployee = await _employeeService.GetProfileEmployee(checkToken.accId);
                    return Ok(profileEmployee);
                }else return BadRequest();
            }
            else return BadRequest();
        }

        [HttpPut("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string mailToken)
        {
            if (!string.IsNullOrEmpty(mailToken))
            {
                var isConfirm = await _tokenService.ConfirmEmailAsync(mailToken);
                if (isConfirm)
                {
                    return Ok("You can login - Welcome....");
                }
                else return BadRequest();
            }
            else return NotFound();
        }

        [HttpPut("UpdateProfileEmployee")]
        public async Task<IActionResult> UpdateProfile(EmployeeUpdateProfileViewModel employeeTmp)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && employeeTmp != null)
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);
                if (checkToken != null)
                {
                    var isUpdate = await _employeeService.UpdateProfle(employeeTmp, checkToken.accId);
                    if (isUpdate)
                    {
                        return Ok("Update Success");
                    }
                    else return BadRequest();
                }
                else return NotFound();               
            }
            else return NoContent();
        }

        [HttpPut("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(string Id)
        {
            string tokenInHeader = Request.Headers["token"].ToString();
            if (!string.IsNullOrEmpty(tokenInHeader) && !string.IsNullOrEmpty(Id))
            {
                var checkToken = await _authService.CheckTokenAsync(tokenInHeader);
                if (checkToken.RoleName == "Admin")
                {
                    var isDelete = await _employeeService.DeleteEmployee(Id);
                    if (isDelete)
                    {
                        return Ok("Delete Success");
                    }else return BadRequest();
                }
                else return NotFound();
            }
            else return NoContent();
        } 
    }
}
