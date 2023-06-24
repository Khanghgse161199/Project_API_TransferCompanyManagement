using AutoMapper;
using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Block;

namespace Services.BlockServices
{
    public interface IBlockServices
    {
        Task<bool> CreateBlockAsync(string name, double weight, string categoryBlockId);
        Task<List<BlockViewModel>> GetAllBlockAsync();
        Task<BlockViewModel> GetBlockByIdAsync(string id);
        Task<bool> UpdateBlockAsync(string id, string name, double weight, string categoryBlockId);
    }
    public class BlockServices: IBlockServices
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;

        public BlockServices(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateBlockAsync(string name, double weight, string categoryBlockId)
        {
            if (!string.IsNullOrEmpty(name) && weight != default(double) && !string.IsNullOrEmpty(categoryBlockId))
            {
                var categoryExist = await _uow.CategoryBlocks.FirstOfDefaultAsync(p => p.Id == categoryBlockId && p.IsActive);
                var ifExist = await _uow.Blocks.FirstOfDefaultAsync(p => p.Name == name);
                if (ifExist == null && categoryExist != null)
                {
                    var newBlock = new Block()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        Weight = weight,
                        CategoryBlockId = categoryBlockId,
                        DateTimeCreate = DateTime.Now,
                        LastUpdate = null
                    };

                    await _uow.Blocks.AddAsync(newBlock);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<BlockViewModel>> GetAllBlockAsync()
        {
            var Blocks = await _uow.Blocks.GetAllAsync();
            if (Blocks.Count > 0)
            {
                return _Mapper.Map<List<BlockViewModel>>(Blocks);   
            }
            else return null;
        }

        public async Task<BlockViewModel> GetBlockByIdAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _Mapper.Map<BlockViewModel>(await _uow.Blocks.FirstOfDefaultAsync(p => p.Id == id));
            }
            else return null;
        }

        public async Task<bool> UpdateBlockAsync(string id, string name, double weight, string categoryBlockId)
        {
            if (!string.IsNullOrEmpty(id))
            {
                OrderShipping currentOrderShipping = null;
                int ifExist = 0;
                try
                {
                    currentOrderShipping = await _uow.OrderShippings.FirstOfDefaultAsync(p => p.BlockId == id && p.IsActive, "WorkMapping");
                    ifExist = (int)currentOrderShipping.WorkMapping.Status;
                    if (currentOrderShipping == null || ifExist == 1)
                    {
                        var currenntBlock = await _uow.Blocks.FirstOfDefaultAsync(p => p.Id == id);
                        if (currenntBlock != null)
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                currenntBlock.Name = name;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            if (weight > 0)
                            {
                                currenntBlock.Weight = weight;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            if (!string.IsNullOrEmpty(categoryBlockId))
                            {
                                currenntBlock.CategoryBlockId = categoryBlockId;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            _uow.Blocks.update(currenntBlock);
                            await _uow.SaveAsync();
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                catch
                {
                    if (currentOrderShipping == null || ifExist == 1)
                    {
                        var currenntBlock = await _uow.Blocks.FirstOfDefaultAsync(p => p.Id == id);
                        if (currenntBlock != null)
                        {
                            if (!string.IsNullOrEmpty(name))
                            {
                                currenntBlock.Name = name;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            if (weight > 0)
                            {
                                currenntBlock.Weight = weight;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            if (!string.IsNullOrEmpty(categoryBlockId))
                            {
                                currenntBlock.CategoryBlockId = categoryBlockId;
                                currenntBlock.LastUpdate = DateTime.Now;
                            }
                            _uow.Blocks.update(currenntBlock);
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
    }
}
