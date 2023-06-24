using AutoMapper;
using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Order;

namespace Services.OrderSerivces
{
    public interface IMainOrderService
    {
        Task<bool> CreateMainOrderAsync(string id, string reciver, string phone, string email, string address, string sender, string reciverCitizenId, decimal total);
        Task<List<MainOrderViewModel>> GetAllMainOrderAsync();
        Task<MainOrderViewModel> GetMainOrderById(string id);
        Task<bool> UpdateIsDoneMainOrderAsync(string id);
        Task<bool> UpdateMainOrderAsync(string id, string reciver, string phone, string email, string address, string sender, string reciverCitizenId, decimal total);
        Task<bool> DeleteMainOrder(string id);
    }
    public class MainOrderService: IMainOrderService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;

        public MainOrderService(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateMainOrderAsync(string id, string reciver, string phone, string email, string address, string sender, string reciverCitizenId, decimal total)
        {
            if (!string.IsNullOrEmpty(reciver) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(sender) && !string.IsNullOrEmpty(reciverCitizenId))
            {
                var newMainOrder = new MainOrder()
                {
                    Id = id,
                    Reciver = reciver,
                    ReciverPhone = phone,
                    ReciverEmail = email,
                    ReciverAddress = address,
                    DateTimeCreate = DateTime.Now,
                    IsActive = true,  
                    IsDone = false,
                    LastUpdate = null,
                    Total = total,
                    Sender = sender,
                    ReciverCitizenId = reciverCitizenId,
                    DateTimeDone = null
                };

                await _uow.MainOrders.AddAsync(newMainOrder);
                await _uow.SaveAsync();
                return true;
            }
            else return false;
        }

        public async Task<List<MainOrderViewModel>> GetAllMainOrderAsync()
        {
            var MainOrders = await _uow.MainOrders.GetAllAsync(p => p.IsActive, o => o.OrderByDescending(p => p.DateTimeCreate));
            if (MainOrders.Count > 0)
            {
                return _Mapper.Map<List<MainOrderViewModel>>(MainOrders);
            }
            else return null;
        }

        public async Task<MainOrderViewModel> GetMainOrderById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                if (currentMainOrder != null)
                {
                    return _Mapper.Map<MainOrderViewModel>(currentMainOrder);
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateIsDoneMainOrderAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.Id == id && p.IsActive, "OrderShippings");
                if(currentMainOrder != null)
                {
                    var ifExist = currentMainOrder.OrderShippings.Where(p => p.IsActive && !p.IsDone).FirstOrDefault();
                    if (ifExist == null)
                    {
                        currentMainOrder.IsDone = true;
                        _uow.MainOrders.update(currentMainOrder);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> UpdateMainOrderAsync(string id, string reciver, string phone, string email, string address, string sender, string reciverCitizenId, decimal total)
        {
            if (!string.IsNullOrEmpty(id))
            {
                bool check = await checkWorking(id);
                if (check)
                {
                    var currenMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.IsActive && !p.IsDone);
                    if (currenMainOrder != null)
                    {
                        if (!string.IsNullOrEmpty(reciver))
                        {
                            currenMainOrder.Reciver = reciver;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(phone))
                        {
                            currenMainOrder.ReciverPhone = phone;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(email))
                        {
                            currenMainOrder.ReciverEmail = email;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(address))
                        {
                            currenMainOrder.ReciverAddress = address;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(sender))
                        {
                            currenMainOrder.Sender = sender;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(reciverCitizenId))
                        {
                            currenMainOrder.ReciverCitizenId = reciverCitizenId;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }
                        if (total > 0)
                        {
                            currenMainOrder.Total = total;
                            currenMainOrder.LastUpdate = DateTime.Now;
                        }

                        _uow.MainOrders.update(currenMainOrder);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteMainOrder(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                bool check = await checkWorking(id);
                if (check)
                {
                    var currentMainOrder = await _uow.MainOrders.FirstOfDefaultAsync(p => p.Id == id && p.IsDone && p.IsActive);
                    if (currentMainOrder != null)
                    {
                        currentMainOrder.IsActive = false;
                        _uow.MainOrders.update(currentMainOrder);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }else return false;
        }

        private async Task<bool> checkWorking(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var orderShipping = await _uow.OrderShippings.GetAllAsync(p => p.MainOrderId == id && p.IsActive, null, "WorkMapping");
                if (orderShipping.Count > 0)
                {
                    var ifExist = orderShipping.Where(p => p.WorkMapping.Status > 1 && p.WorkMapping.IsActive).FirstOrDefault();
                    if (ifExist == null)
                    {
                        return true;
                    }
                    else return false;
                }
                else return true;
            }else return false;
        }
    }
}
