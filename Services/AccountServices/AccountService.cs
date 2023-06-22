using DataService.Entities;
using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AccountServices
{
    public interface IAccountService
    {
        Task<bool> CreateAccountAsync(string username, string password, string rolename, string accId);
    }


    public class AccountService: IAccountService
    {
        private string key = "ThisIsPassword:Aibietgidau:))06/10/2023";
        private IHashService _hashService;
        private readonly IUnitOfWork _uow;
        public AccountService(IUnitOfWork unitOfWork, IHashService hashService)
        {
            _uow = unitOfWork;
            _hashService = hashService;
        }

        public async Task<bool> CreateAccountAsync(string username, string password, string rolename,string accId)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(rolename))
            {
                
                var role = await _uow.Roles.FirstOfDefaultAsync(p => p.Name == rolename);
                if (role != null)
                {
                    Account newAccount = new Account()
                    {
                        Id = accId,
                        Username = username,
                        Password = _hashService.SHA256(password + key),
                        RoleId = role.Id,
                        IsActive = true
                    };
                    await _uow.Accounts.AddAsync(newAccount);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
