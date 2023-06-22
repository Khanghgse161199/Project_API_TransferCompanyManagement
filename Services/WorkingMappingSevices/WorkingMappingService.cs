using AutoMapper;
using DataService.Repositories.UnitOfWork;
using DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.WorkMapping;
using Microsoft.IdentityModel.Tokens;
using Services.RatingServices;

namespace Services.WorkingMappingSevices
{
    public interface IWorkingMappingService
    {
        Task<bool> CreateWorkingMappingAsync(string employeeId, string transitCarId, string containerId);
        Task<List<WorkMappingViewModel>> GetAllWorkMappingAsync();
        Task<WorkMappingViewModel> GetWorkingMappingByIdAsync(string id);
        Task<List<WorkMappingViewModel>> GetAllWorkMappingToRegisterAsync();
        Task<List<WorkingMappingOfEmployeeViewModel>> GetAllWorkingMappingOfEmployeeAsync(string employeeId);
        Task<bool> registerEmployeeWorkingMappingAsync(string idEmployee, string workingMappingId);
        Task<bool> UpdateEmployeeWorkingMappingAsync(string id, string employeeId);
        Task<bool> UpdateStatusWorkingMappingAsync(string id, status tmp);
        Task<bool> DeletWorkingMappingAsync(string id);
    }
    public class WorkingMappingService : IWorkingMappingService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        private readonly IRatingService _ratingService;

        public WorkingMappingService(IUnitOfWork uow, IRatingService ratingService)
        {
            _uow = uow;
            _ratingService = ratingService;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateWorkingMappingAsync(string employeeId, string transitCarId, string containerId)
        {
            var currentTransitCar = await _uow.TransitCars.FirstOfDefaultAsync(p => p.Id == transitCarId && p.IsActive && !p.IsWorking);
            var currentContainer = await _uow.Containers.FirstOfDefaultAsync(p => p.Id == containerId && p.IsActve && !p.IsWorking);
            if (currentTransitCar != null && currentContainer != null)
            {
                var ratingId = Guid.NewGuid().ToString();
                var newWorkingMapping = new DataService.Entities.WorkMapping()
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = employeeId,
                    TransitCarId = transitCarId,
                    ContainerId = containerId,
                    Status = (int)status.UnActive,
                    IsActive = true,
                    DateTimeCreate = DateTime.Now,
                    LastUpdate = null,                  
                };
                var isCreate = await _ratingService.CreateRatingAsync(ratingId);
                if (isCreate)
                {
                    currentTransitCar.IsWorking = true;
                    currentContainer.IsWorking = true;
                    await _uow.WorkingMappings.AddAsync(newWorkingMapping);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<WorkMappingViewModel>> GetAllWorkMappingAsync()
        {
            var WorkMappings = await _uow.WorkingMappings.GetAllAsync(p => p.IsActive);
            if (WorkMappings.Count > 0)
            {
                return _Mapper.Map<List<WorkMappingViewModel>>(WorkMappings);
            }
            else return null;
        }

        public async Task<List<WorkMappingViewModel>> GetAllWorkMappingToRegisterAsync()
        {
            var WorkMappings = await _uow.WorkingMappings.GetAllAsync(p => p.IsActive && p.EmployeeId == null && p.Status == 1, o => o.OrderBy(p => p.DateTimeCreate));
            if (WorkMappings.Count > 0)
            {
                return _Mapper.Map<List<WorkMappingViewModel>>(WorkMappings);
            }
            else return null;
        }

        public async Task<WorkMappingViewModel> GetWorkingMappingByIdAsync(string id) {
            if (!string.IsNullOrEmpty(id))
            {
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id);
                if (currentWorkingMapping != null)
                {
                    return _Mapper.Map<WorkMappingViewModel>(currentWorkingMapping);
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<WorkingMappingOfEmployeeViewModel>> GetAllWorkingMappingOfEmployeeAsync(string employeeId)
        {
            if (!string.IsNullOrEmpty(employeeId))
            {
                var workingMappings = await _uow.WorkingMappings.GetAllAsync(p => p.EmployeeId == employeeId && p.IsActive && p.Status > 1, o => o.OrderBy(p => p.Status));
                if (workingMappings.Count > 0)
                {
                    return _Mapper.Map<List<WorkingMappingOfEmployeeViewModel>>(workingMappings);
                }
                else return null;
            }
            else return null;
        }
        public async Task<bool> UpdateEmployeeWorkingMappingAsync(string id, string employeeId) {
            if (!string.IsNullOrEmpty(id))
            {
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive && p.EmployeeId != null && p.Status == 1);
                if (currentWorkingMapping != null )
                {
                    if (!string.IsNullOrEmpty(employeeId))
                    {
                        currentWorkingMapping.EmployeeId = employeeId;
                        currentWorkingMapping.LastUpdate = DateTime.Now;
                    }

                    _uow.WorkingMappings.update(currentWorkingMapping);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }else return false;
        }

        public async Task<bool> UpdateStatusWorkingMappingAsync(string id, status tmp)
        {
            if (!string.IsNullOrEmpty(id) && tmp != null && (int)tmp > 1 && (int)tmp <= 3)
            {
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id && p.Status > 1 && p.IsActive && p.EmployeeId != null, "OrderShippings,TransitCar,Container,Employee");
                if (currentWorkingMapping != null)
                {
                    if ((int)tmp == 3)
                    {
                        var ifExist = currentWorkingMapping.OrderShippings.Where(p => p.IsActive && !p.IsDone).FirstOrDefault();
                        if (ifExist == null)
                        {
                            currentWorkingMapping.Status = 2;
                            _uow.WorkingMappings.update(currentWorkingMapping);
                            currentWorkingMapping.Container.IsWorking = false;
                            _uow.Containers.update(currentWorkingMapping.Container);
                            currentWorkingMapping.TransitCar.IsWorking = false;
                            _uow.TransitCars.update(currentWorkingMapping.TransitCar);
                            currentWorkingMapping.Employee.IsWorking = false;
                            _uow.Employees.Udpate(currentWorkingMapping.Employee);
                            await _uow.SaveAsync();
                            return true;
                        }
                        else return false;  
                    }
                    if ((int)tmp == 2)
                    {
                        currentWorkingMapping.Status = 2;
                        _uow.WorkingMappings.update(currentWorkingMapping);
                        currentWorkingMapping.Container.IsWorking = true;
                        _uow.Containers.update(currentWorkingMapping.Container);
                        currentWorkingMapping.TransitCar.IsWorking = true;
                        _uow.TransitCars.update(currentWorkingMapping.TransitCar);
                        currentWorkingMapping.Employee.IsWorking = true;
                        _uow.Employees.Udpate(currentWorkingMapping.Employee);                     
                        await _uow.SaveAsync();
                        return true;
                    }
                    else if ((int)tmp == 1)
                    {
                        currentWorkingMapping.Status = 1;
                        _uow.WorkingMappings.update(currentWorkingMapping);
                        currentWorkingMapping.Container.IsWorking = false;
                        _uow.Containers.update(currentWorkingMapping.Container);
                        currentWorkingMapping.TransitCar.IsWorking = false;
                        _uow.TransitCars.update(currentWorkingMapping.TransitCar);
                        currentWorkingMapping.Employee.IsWorking = false;
                        _uow.Employees.Udpate(currentWorkingMapping.Employee);                      
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }

        public async Task<bool> DeletWorkingMappingAsync(string id) {
            if (!string.IsNullOrEmpty(id))
            {
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id && p.Status == 3 && p.IsActive);
                if (currentWorkingMapping != null)
                {
                    currentWorkingMapping.IsActive = false;
                    _uow.WorkingMappings.update(currentWorkingMapping);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> registerEmployeeWorkingMappingAsync(string idEmployee, string workingMappingId)
        {
            if(!string.IsNullOrEmpty(idEmployee) && !string.IsNullOrEmpty(workingMappingId)) {
                var currentEmployee = await _uow.Employees.FirstOfDefaultAsync(p => p.Id == idEmployee && !p.IsWorking);
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == workingMappingId && p.IsActive && p.Status == 1);
                if (currentEmployee != null && currentWorkingMapping != null)
                {
                    currentWorkingMapping.EmployeeId = currentEmployee.Id;
                    currentEmployee.IsWorking = true;
                    _uow.Employees.Udpate(currentEmployee);
                    _uow.WorkingMappings.update( currentWorkingMapping);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }else return false;
        }
    }
}
