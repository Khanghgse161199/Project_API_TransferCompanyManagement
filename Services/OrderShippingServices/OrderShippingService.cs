using AutoMapper;
using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using MailKit.Search;
using Services.RatingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.OrderDetail;

namespace Services.OrderDetailServices
{
    public interface IOrderDetailService
    {
        Task<bool> CreateOrderDetailAsync(string orderId, string blockId, string workMappingId, decimal price);
        Task<List<OrderShippingViewModel>> GetAllOrderDetailAsync();
        Task<OrderShippingViewModel> GetOrderDetailByIdAsync(string id);
        Task<List<OrderShippingViewModel>> GetOrederShippingByMainOrder(string id);
        Task<List<OrderShippingViewModel>> GetOrderDetailByWorkingMappingAsync(string id);
        Task<bool> UpdateOrderDetailAsync(string id, string orderId, string blockId, string workMappingId, decimal price);
        Task<bool> UpdateIsDoneAsync(string id);
        Task<bool> DeleteOrderDetailAsync(string id);
        
    }
    public class OrderShippingService: IOrderDetailService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        private readonly IRatingService _rating;

        public OrderShippingService(IUnitOfWork uow, IRatingService ratingService)
        {
            _uow = uow;
            _rating = ratingService;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateOrderDetailAsync(string orderId, string blockId, string workMappingId, decimal price)
        {
            if (!string.IsNullOrEmpty(orderId) && !string.IsNullOrEmpty(blockId) && !string.IsNullOrEmpty(workMappingId) && price > 0)
            {
                var currentWorkMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == workMappingId && p.Status == 1 && p.IsActive, "Container");
                var currentBlock = await _uow.Blocks.FirstOfDefaultAsync(p => p.Id == blockId);
                var currentMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.Id == orderId && p.IsActive && !p.IsDone);
                if(currentWorkMapping != null && currentBlock != null && currentMainOrder != null)
                {
                    
                        if(currentWorkMapping.Container.Weight - currentBlock.Weight > 0)
                        {
                            var newOrderDetail = new OrderShipping()
                            {
                                Id = Guid.NewGuid().ToString(),
                                MainOrderId = orderId,
                                DateTimeRecive = null,
                                IsActive = true,
                                IsDone = false,
                                WorkMappingId = workMappingId,
                                Price = price,
                                BlockId = blockId,
                                LastUpdate = null,
                                DateTimeCreate = DateTime.Now,                          
                            };
                            await _uow.OrderShippings.AddAsync(newOrderDetail);
                            await _uow.SaveAsync();
                            return true;                                           
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<List<OrderShippingViewModel>> GetAllOrderDetailAsync()
        {
            var orderDetails = await _uow.OrderShippings.GetAllAsync();
            if (orderDetails.Count > 0)
            {
                return _Mapper.Map<List<OrderShippingViewModel>>(orderDetails);
            }
            else return null;
        }

        public async Task<OrderShippingViewModel> GetOrderDetailByIdAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return _Mapper.Map<OrderShippingViewModel>(await _uow.OrderShippings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive));
            }
            else return null;
        }

        public async Task<List<OrderShippingViewModel>> GetOrederShippingByMainOrder(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.Id == id && p.IsActive, "OrderShippings");
                if (currentMainOrder != null)
                {
                    return _Mapper.Map<List<OrderShippingViewModel>>(currentMainOrder.OrderShippings.OrderBy(p => p.IsDone));
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<OrderShippingViewModel>> GetOrderDetailByWorkingMappingAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive, "OrderShippings");
                if (currentWorkingMapping != null)
                {
                    return _Mapper.Map<List<OrderShippingViewModel>>(currentWorkingMapping.OrderShippings.OrderBy(p => p.IsDone));
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateOrderDetailAsync(string id, string orderId, string blockId, string workMappingId, decimal price)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentOrderDetail = await _uow.OrderShippings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive && p.IsDone == true, "WorkMapping");
                if (currentOrderDetail != null && currentOrderDetail.WorkMapping.Status == 1 && currentOrderDetail.WorkMapping.IsActive)
                {
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        currentOrderDetail.MainOrderId = orderId;
                        currentOrderDetail.LastUpdate = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(blockId))
                    {
                        currentOrderDetail.BlockId = blockId;
                        currentOrderDetail.LastUpdate = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(workMappingId))
                    {
                        currentOrderDetail.WorkMappingId = workMappingId;
                        currentOrderDetail.LastUpdate = DateTime.Now;
                    }
                    if(price > 0)
                    {
                        currentOrderDetail.Price = price;
                        currentOrderDetail.LastUpdate = DateTime.Now;
                    }
                    _uow.OrderShippings.update(currentOrderDetail);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> UpdateIsDoneAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentOrderDetail = await _uow.OrderShippings.FirstOfDefaultAsync(p => p.Id == id && !p.IsDone && p.IsActive, "WorkMapping");
                if (currentOrderDetail != null)
                {
                    currentOrderDetail.IsDone = true;
                    _uow.OrderShippings.update(currentOrderDetail);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteOrderDetailAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentOrderDetail = await _uow.OrderShippings.FirstOfDefaultAsync(p => p.Id == id && p.IsDone == true && p.IsActive, "WorkMapping");
                if (currentOrderDetail != null && currentOrderDetail.WorkMapping.Status == 1 && currentOrderDetail.WorkMapping.IsActive)
                {
                    currentOrderDetail.IsActive = false;
                    _uow.OrderShippings.update(currentOrderDetail);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
    }
}
