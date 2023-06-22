using DataService.Entities;
using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.EmployeeViewModel;
using ViewModel.TokenViewModel;

namespace Services.AuthServices
{
    public interface IAuthService
    {
        Task<ResultCheckToken> CheckTokenAsync(string token);
        Task<LoginResultEmployeeViewModel> CheckLoginAsync(LoginEmployeeViewModel loginEmployeeViewModel);
        Task<bool> ConfirmEmailAsync(string mailToken);
        Task<bool> ReSendMailConfirmAsync(string mailReciver);
    }
    public class AuthService: IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHashService _hashService;
        private string key = "ThisIsPassword:Aibietgidau:))06/10/2023";
        public AuthService(IUnitOfWork uow, IHashService hashService)
        {
            _uow = uow;
            _hashService = hashService;
        }

        public async Task<LoginResultEmployeeViewModel> CheckLoginAsync(LoginEmployeeViewModel loginEmployeeViewModel)
        {
            if (!string.IsNullOrEmpty(loginEmployeeViewModel.username) && !string.IsNullOrEmpty(loginEmployeeViewModel.password))
            {
                var hasAcc = await _uow.Accounts.FirstOfDefaultAsync(p => p.Username == loginEmployeeViewModel.username && p.Password == _hashService.SHA256(loginEmployeeViewModel.password + key) && p.IsActive, "Role");
                if (hasAcc.Role.Name == "Admin" || hasAcc.Role.Name == "Employee")
                {
                    var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccId == hasAcc.Id && p.EtisActive, "Acc");
                    if (currentToken != null)
                    {                     
                        if (currentToken.EtisActive)
                        {
                            string newAccessToken = Guid.NewGuid().ToString();
                            currentToken.AccessToken = newAccessToken;
                            currentToken.CreateDate = DateTime.Now;
                            currentToken.AccessTokenIsActive = true;
                            _uow.Tokens.Udpate(currentToken);
                            await _uow.SaveAsync();
                            return new LoginResultEmployeeViewModel()
                            {
                                AccessToken = newAccessToken
                            };
                        }
                        else return null;
                    }
                    else return null;
                }
                else return null;
            } 
            else return null;
        }

        public async Task<bool> ConfirmEmailAsync(string mailToken)
        {
            var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.EmailToken == mailToken);
            if (currentToken != null)
            {
                var expiredTime = currentToken.EtcreatedDate.AddMinutes(15);
                if (currentToken.EtcreatedDate <= expiredTime)
                {
                    currentToken.EtisActive = true;
                    _uow.Tokens.Udpate(currentToken);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> ReSendMailConfirmAsync(string mailReciver)
        {
            var currentEmployee = await _uow.Employees.FirstOfDefaultAsync(p => p.Email == mailReciver, "Acc");
            if (currentEmployee != null)
            {
                var currentToken = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccId == currentEmployee.AccId && !currentEmployee.Acc.IsActive && p.EtcreatedDate < DateTime.Now);
                if (currentToken != null)
                {
                    string newEToken = Guid.NewGuid().ToString();
                    currentToken.EmailToken = newEToken;
                    currentToken.EtcreatedDate = DateTime.Now;
                    _uow.Tokens.Udpate(currentToken);
                    await _uow.SaveAsync();
                    //await SendMailConfirmAsync(currentEmployee.Email, newEToken);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<ResultCheckToken> CheckTokenAsync(string token)
        {
            var tokenDb = await _uow.Tokens.FirstOfDefaultAsync(p => p.AccessToken == token && p.AccessTokenIsActive,"Acc");
            if (tokenDb != null)
            {
                var Role = await _uow.Roles.FirstOfDefaultAsync(p => p.Id == tokenDb.Acc.RoleId && p.IsActive);
                if (Role != null)
                {
                    return new ResultCheckToken()
                    {
                        accId = tokenDb.AccId,
                        Username = tokenDb.Acc.Username,
                        RoleId = Role.Id,
                        RoleName = Role.Name                                             
                    };
                }
                else return null;
            } else return null;           
        }


    }
}
