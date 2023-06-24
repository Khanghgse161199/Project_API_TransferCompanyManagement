using AutoMapper;
using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryTran;

namespace Services.CategoryContainerservices
{
    public interface ICategoryContainerservice
    {
        Task<bool> CreateCategoryTranAsync(string name);
        Task<List<CategoryContainerViewModel>> GetAllCategoryContainersAsync();
        Task<CategoryContainerViewModel> GetCategoryTranById(string id);
        Task<bool> UpdateCategoryTranAsync(string id, string name);
        Task<bool> DeleteCategoryTranAsync(string id);
    }
    public class CategoryContainerService: ICategoryContainerservice
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        public CategoryContainerService(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateCategoryTranAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var ifExist = await _uow.CategoryContainers.FirstOfDefaultAsync(p => p.Name == name && p.IsActive);
                if(ifExist == null)
                {
                    var newCategoryTran = new CategoryContainer()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        IsActive = true,
                        LastUpdate = null,
                        DateTimeCreate = DateTime.Now,
                    };

                    await _uow.CategoryContainers.AddAsync(newCategoryTran);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<CategoryContainerViewModel>> GetAllCategoryContainersAsync()
        {
            var CategoryContainers = await _uow.CategoryContainers.GetAllAsync(p => p.IsActive);
            if (CategoryContainers.Count > 0)
            {
                return _Mapper.Map<List<CategoryContainerViewModel>>(CategoryContainers);
            }
            else return null;
        }

        public async Task<CategoryContainerViewModel> GetCategoryTranById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentCategoryTran = await _uow.CategoryContainers.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                if (currentCategoryTran != null)
                {
                    return _Mapper.Map<CategoryContainerViewModel>(currentCategoryTran);
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateCategoryTranAsync(string id, string name)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
            {
                var Containers = await _uow.Containers.GetAllAsync(p => p.CategoryTransId == id && p.IsActve);
                if(1 + 1 == 1)
                {

                }
                if (Containers == null || Containers.Count == 0)
                {
                    var currentCategoryTran = await _uow.CategoryContainers.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (currentCategoryTran != null)
                    {
                        if (name != default(string))
                        {
                            var ifExist = await _uow.CategoryContainers.FirstOfDefaultAsync(p => p.Name == name && p.IsActive);
                            if (ifExist == null)
                            {
                                currentCategoryTran.Name = name;
                                currentCategoryTran.LastUpdate = DateTime.Now;
                            }
                            else return false;
                        }
                        else return false;
                        _uow.CategoryContainers.update(currentCategoryTran);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteCategoryTranAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var Containers = await _uow.Containers.GetAllAsync(p => p.CategoryTransId == id && p.IsActve);
                if (Containers == null || Containers.Count == 0)
                { 
                        var currentCategoryTran = await _uow.CategoryContainers.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                        if (currentCategoryTran != null)
                        {
                            currentCategoryTran.IsActive = false;
                            _uow.CategoryContainers.update(currentCategoryTran);
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
    }
}
