using AutoMapper;
using DataService.Entities;
using DataService.HashService;
using DataService.Repositories.AccountRepository;
using DataService.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Services.AccountServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.EmployeeViewModel;
using VireModel.EmployeeViewModel;


namespace Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<bool> CreateEmployeeAsync(CreateEmployeeViewModel createEmployeeViewModel, string AccId, string mailToken);
        Task<List<EmployeeViewModel>> GetAllEmployeeAsync();
        Task<EmployeeViewModel> GetEmployeeByIdAsync(string employeeId);
        Task<EmployeeProfileVIewModel> GetProfileEmployee(string employeeId);
        Task<bool> UpdateProfle(EmployeeUpdateProfileViewModel employeeTmp, string employeeId);
        Task<bool> DeleteEmployee(string employeeId);
    }
    public class EmployeeService: IEmployeeService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _mapper;
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingService.MappingService>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<bool> CreateEmployeeAsync(CreateEmployeeViewModel createEmployeeViewModel, string AccId, string mailToken)
        {
            if (createEmployeeViewModel != null && createEmployeeViewModel.Username != "string" && createEmployeeViewModel.Password != "string") {
                var checkUserName = await _uow.Accounts.FirstOfDefaultAsync(p => p.Username == createEmployeeViewModel.Username);
                if (checkUserName != null)
                {
                    if (!string.IsNullOrEmpty(AccId))
                    {
                        var newEmpployee = new Employee()
                        {
                            Id = Guid.NewGuid().ToString(),
                            AccId = AccId,
                            FullName = createEmployeeViewModel.FullName,
                            Address = createEmployeeViewModel.Address,
                            Phone = createEmployeeViewModel.Phone,
                            Email = createEmployeeViewModel.Email,
                            CitizenId = createEmployeeViewModel.CitizenId,
                            IsWorking = false
                            
                        };
                        var newToken = new Token
                        {
                            Id = Guid.NewGuid().ToString(),
                            AccId = AccId,
                            AccessToken = "",
                            EmailToken = mailToken,
                            EtcreatedDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            AccessTokenIsActive = false,
                            EtisActive = false,
                        };
                        await _uow.Employees.AddAsync(newEmpployee);
                        await _uow.Tokens.AddAsync(newToken);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;              
            }else return false;
        }

        public async Task<List<EmployeeViewModel>> GetAllEmployeeAsync()
        {
            var allEmployees = await _uow.Employees.GetAllAsync(p => p.Acc.IsActive, o => o.OrderBy(p => p.SummayRating));
            return _mapper.Map<List<EmployeeViewModel>>(allEmployees);
        }

        public async Task<EmployeeViewModel> GetEmployeeByIdAsync(string employeeId)
        {
            var employee = await _uow.Employees.FirstOfDefaultAsync(p => p.Id == employeeId && p.Acc.IsActive);
            return _mapper.Map<EmployeeViewModel>(employee);
        }

        public async Task<EmployeeProfileVIewModel> GetProfileEmployee(string AccEmployeeId)
        {
            var employee = await _uow.Employees.FirstOfDefaultAsync(p => p.Acc.Id == AccEmployeeId && p.Acc.IsActive, "Acc");
            var newEmployee =  _mapper.Map<EmployeeProfileVIewModel>(employee);
            newEmployee.username = employee.Acc.Username;
            return newEmployee;
        }

        public async Task<bool> UpdateProfle(EmployeeUpdateProfileViewModel employeeTmp, string AccId)
        {
            if (employeeTmp != null && !string.IsNullOrEmpty(AccId))
            {
                var employeeCurrent = await _uow.Employees.FirstOfDefaultAsync(p => p.AccId == AccId && p.Acc.IsActive);
                if (employeeCurrent != null)
                {
                    if (!string.IsNullOrEmpty(employeeTmp.FullName) && employeeTmp.FullName != "string")
                    {
                        employeeCurrent.FullName = employeeTmp.FullName;
                    }
                    if (!string.IsNullOrEmpty(employeeTmp.Phone))
                    {
                        employeeCurrent.Phone = employeeTmp.Phone;
                    }
                    if (!string.IsNullOrEmpty(employeeTmp.Email))
                    {
                        employeeCurrent.Email = employeeTmp.Email;
                    }
                    if (!string.IsNullOrEmpty(employeeTmp.Address) && employeeTmp.Address != "string")
                    {
                        employeeCurrent.Address = employeeTmp.Address;
                    }
                    if (!string.IsNullOrEmpty(employeeTmp.CitizenId) && employeeTmp.CitizenId != "string")
                    {
                        employeeCurrent.CitizenId = employeeTmp.CitizenId;
                    }
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteEmployee(string employeeId)
        {
            var employee = await _uow.Employees.FirstOfDefaultAsync(p => p.Id == employeeId && p.Acc.IsActive,"Acc");
            if(employee != null)
            {
                employee.Acc.IsActive = false;
                _uow.Employees.Udpate(employee);
                await _uow.SaveAsync();
                return true;
            }else return false;
        }   
    }
}
