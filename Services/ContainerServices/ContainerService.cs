using AutoMapper;
using DataService.Repositories.UnitOfWork;
using DataService.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryTran;
using ViewModel.Container;

namespace Services.ContainerServices
{
    public interface IContainerService
    {
        Task<bool> CreateContainerAysnc(string name, string categoryTranId, double weight);
        Task<List<ContainerViewModel>> GetAllContainerAsync();
        Task<bool> DeleteContainerAsync(string id);
        Task<bool> UpdateContainerAsync(string id, string name, string categoryId, double weight);
        Task<ContainerViewModel> GetContainerByIdAsync(string id);
    }
    public class ContainerService: IContainerService
    {
        private readonly IUnitOfWork _uow;
        public ContainerService(IUnitOfWork uow)
        {
            _uow = uow;            
        }

        public async Task<bool> CreateContainerAysnc(string name, string categoryTranId,double weight)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(categoryTranId) && weight != default(double))
            {
                var ifExist = await _uow.Containers.FirstOfDefaultAsync(p => p.Name == name && p.IsActve);
                if (ifExist == null)
                {
                    var newContainer = new DataService.Entities.Container()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CategoryTransId = categoryTranId,
                        Name = name,
                        IsWorking = false,
                        Weight = weight,
                        IsActve = true,
                        LastUpdate = null,
                        DateTimeCreate = DateTime.Now,
                        
                    };

                    await _uow.Containers.AddAsync(newContainer);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }else return false;
        }

        public async Task<List<ContainerViewModel>> GetAllContainerAsync()
        {
            var containers = await _uow.Containers.GetAllAsync(p => p.IsActve, null,"CategoryTrans");
            if (containers.Count > 0)
            {
                var listResult = containers.Select(p => new ContainerViewModel
                {
                    Id = p.Id,
                    name = p.Name,
                    CategoryName = p.CategoryTrans.Name,
                    Weight = p.Weight,  
                    IsWorking = p.IsWorking,
                    DateCreate = p.DateTimeCreate
                });

                if (listResult.ToList().Count > 0)
                {
                    return listResult.ToList();
                }
                else return null;
            }
            else return null;
        }
        public async Task<ContainerViewModel> GetContainerByIdAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentContainer = await _uow.Containers.FirstOfDefaultAsync(p => p.Id == id && p.IsActve, "CategoryTrans");
                if (currentContainer != null)
                {
                    return new ContainerViewModel() { 
                        Id = currentContainer.Id,
                        name = currentContainer.Name,
                        Weight= currentContainer.Weight,
                        CategoryName = currentContainer.CategoryTrans.Name,
                        IsWorking = currentContainer.IsWorking,
                        DateCreate = currentContainer.DateTimeCreate
                    };
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateContainerAsync(string id, string name, string categoryId, double weight)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var checkExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.ContainerId == id && p.Status > 1 && p.IsActive);
                if (checkExist == null) {
                    var currentContainer = await _uow.Containers.FirstOfDefaultAsync(p => p.Id == id && p.IsActve, "CategoryTrans");
                    if (currentContainer != null)
                    {
                        if (!string.IsNullOrEmpty(categoryId))
                        {
                            currentContainer.CategoryTransId = categoryId;
                            currentContainer.LastUpdate = DateTime.Now;
                        }
                        if (weight > 0)
                        {
                            currentContainer.Weight = weight;
                            currentContainer.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(name))
                        {
                            var ifExist = await _uow.Containers.FirstOfDefaultAsync(p => p.Name == name && p.IsActve);
                            if (ifExist == null)
                            {
                                currentContainer.Name = name;
                                currentContainer.LastUpdate = DateTime.Now;
                            }
                            else return false;
                        }

                        _uow.Containers.update(currentContainer);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteContainerAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var checkExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.ContainerId == id && p.Status > 1 && p.IsActive);
                if (checkExist == null)
                {
                    var currentContainer = await _uow.Containers.FirstOfDefaultAsync(p => p.Id == id && p.IsActve);
                    if (currentContainer != null)
                    {                
                         currentContainer.IsActve = false;
                         _uow.Containers.update(currentContainer);
                         await _uow.SaveAsync();
                         return true;                          
                    }else return false;
                }
                else return false;
            }
            else return false;
        }

        //public async Task<bool> UpdateIsWorkingContainer(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var checkExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.ContainerId == id && p.Status > 1 && p.IsActive);
        //        if (checkExist == null)
        //        {
        //            var currentContainer = await _uow.Containers.FirstOfDefaultAsync(p => p.Id == id && p.IsActve, "WorkMappings");
        //            if (currentContainer != null)
        //            {
        //                var temp = currentContainer.WorkMappings.Where(p => p.IsDone && p.IsActive).FirstOrDefault();
        //                if (temp == null)
        //                {
        //                    currentContainer.IsWorking = !currentContainer.IsWorking;
        //                    _uow.Containers.update(currentContainer);
        //                    await _uow.SaveAsync();
        //                    return true;
        //                }
        //                else return false;
        //            }
        //            else return false;
        //        }
        //        else return false;
        //    }
        //    else return false;
        //}
    }
}
