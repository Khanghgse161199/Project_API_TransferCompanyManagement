using AutoMapper;
using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.CategoryBlock;
using ViewModel.CategoryTran;

namespace Services.CategoryBlockServices
{
    public interface ICategoryBlockService
    {
        Task<bool> CreateCategoryBlockAsync(string name);
        Task<List<CategoryBlockViewModel>> GetAllCategoryBlockAsync();
        Task<CategoryBlockViewModel> GetCategoryBlockById(string id);
        Task<bool> UpdateCategoryBlockAsync(string id, string name);
        Task<bool> DeleteCategoryBlockAsync(string id);
    }
    public class CategoryBlockService: ICategoryBlockService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        public CategoryBlockService(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateCategoryBlockAsync(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var ifExist = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Name == name && p.IsActive);
                if (ifExist == null)
                {
                    var newCategoryBlock = new CategoryBlock()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        IsActive = true,
                        LastUpdate = null,
                        DateTimeCreate = DateTime.Now,
                    };

                    await _uow.CategoryBlocks.AddAsync(newCategoryBlock);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<CategoryBlockViewModel>> GetAllCategoryBlockAsync()
        {
            var categoryBlocks = await _uow.CategoryBlocks.GetAllAsync(p => p.IsActive);
            if (categoryBlocks.Count > 0)
            {
                return _Mapper.Map<List<CategoryBlockViewModel>>(categoryBlocks);
            }
            else return null;
        }

        public async Task<CategoryBlockViewModel> GetCategoryBlockById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentCategoryBlock = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                if (currentCategoryBlock != null)
                {
                    return _Mapper.Map<CategoryBlockViewModel>(currentCategoryBlock);
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateCategoryBlockAsync(string id, string name)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(name))
            {
                var OrderDetails = await _uow.OrderShippings.GetAllAsync(p => p.IsActive, null, "Block");
                if (OrderDetails.Count > 0)
                {
                    var existOderDetail = OrderDetails.Where(p => p.Block.CategoryBlockId == id && p.IsActive).FirstOrDefault();
                    if (existOderDetail == null)
                    {
                        var currentCategoryBlock = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                        if (currentCategoryBlock != null)
                        {
                            var ifExist = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Name == name && p.IsActive);
                            if (ifExist == null)
                            {
                                currentCategoryBlock.Name = name;
                                currentCategoryBlock.LastUpdate = DateTime.Now;
                                _uow.CategoryBlocks.update(currentCategoryBlock);
                                await _uow.SaveAsync();
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                    else return false;
                }
                else
                {
                    var currentCategoryBlock = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (currentCategoryBlock != null)
                    {
                        var ifExist = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Name == name && p.IsActive);
                        if (ifExist == null)
                        {
                            currentCategoryBlock.Name = name;
                            currentCategoryBlock.LastUpdate = DateTime.Now;
                            _uow.CategoryBlocks.update(currentCategoryBlock);
                            await _uow.SaveAsync();
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
            }
            else return false;
        }

        public async Task<bool> DeleteCategoryBlockAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var OrderDetails = await _uow.OrderShippings.GetAllAsync(p => p.IsActive && !p.IsDone, null, "Block");
                if (OrderDetails.Count > 0)
                {
                    var existOderDetail = OrderDetails.Where(p => p.Block.CategoryBlockId == id && p.IsActive).FirstOrDefault();
                    if (existOderDetail == null)
                    {
                        var currentCategoryBlock = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                        if (currentCategoryBlock != null)
                        {
                            currentCategoryBlock.IsActive = false;
                            _uow.CategoryBlocks.update(currentCategoryBlock);
                            await _uow.SaveAsync();
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else {
                    var currentCategoryBlock = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (currentCategoryBlock != null)
                    {
                        currentCategoryBlock.IsActive = false;
                        _uow.CategoryBlocks.update(currentCategoryBlock);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }          
            }
            else return false;
        }
    }
}
