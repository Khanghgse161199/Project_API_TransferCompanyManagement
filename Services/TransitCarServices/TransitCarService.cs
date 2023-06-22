using DataService.Entities;
using DataService.Repositories.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.TransitCar;
using AutoMapper;
using Services.MappingService;

namespace Services.TransitCarServices
{
    public interface ITransitCarService
    {
        Task<bool> CreateTransitCarAsync(string name, string brand, DateTime registerDate, DateTime outOfDate, string productionCompany);
        Task<List<TransitCarViewModel>> GetAllTransitCarAsync();
        Task<TransitCarViewModel> GetTransitCarById(string id);
        Task<bool> UpdateTransitCarAsync(string id, string name, string brand, string originCompany, DateTime dateRegister, DateTime outOfDate);
        Task<bool> DeleteTransitCarAsync(string id);
    }
    public class TransitCarService : ITransitCarService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        
        public TransitCarService(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateTransitCarAsync(string name, string brand, DateTime registerDate, DateTime outOfDate, string productionCompany)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(brand) && !string.IsNullOrEmpty(productionCompany) && registerDate != default(DateTime) && outOfDate != default(DateTime))
            {
                TransitCar newTransitCar = new TransitCar() {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Brand = brand,
                    DateRegister = registerDate,
                    OutOfDate = outOfDate,
                    OriginCompany = productionCompany,
                    IsActive = true,
                    LastUpdate = null,
                    IsWorking = false
                };
                await _uow.TransitCars.AddAsync(newTransitCar);
                await _uow.SaveAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<TransitCarViewModel>> GetAllTransitCarAsync()
        {
           var transitCars = await _uow.TransitCars.GetAllAsync(p => p.IsActive);
            if (transitCars.Count > 0)
            {
                return _Mapper.Map<List<TransitCarViewModel>>(transitCars);
            }
            else return null;
        }

        public async Task<TransitCarViewModel> GetTransitCarById(string id)
        {
            return _Mapper.Map<TransitCarViewModel>(await _uow.TransitCars.FirstOfDefaultAsync(p => p.Id == id && p.IsActive));
        }

        public async Task<bool> UpdateTransitCarAsync(string id, string name, string brand, string originCompany, DateTime dateRegister, DateTime outOfDate)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ifExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.TransitCarId == id && p.Status > 1 && p.IsActive);
                if (ifExist == null)
                {

                    var currentTransitCar = await _uow.TransitCars.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (currentTransitCar != null)
                    {
                        if (!string.IsNullOrEmpty(name) && name != default(string))
                        {
                            currentTransitCar.Name = name;
                            currentTransitCar.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(brand) && brand != default(string))
                        {
                            currentTransitCar.Brand = brand;
                            currentTransitCar.LastUpdate = DateTime.Now;
                        }
                        if (!string.IsNullOrEmpty(originCompany) && originCompany != default(string))
                        {
                            currentTransitCar.OriginCompany = originCompany;
                            currentTransitCar.LastUpdate = DateTime.Now;
                        }
                        if (dateRegister != default(DateTime) && dateRegister != null)
                        {
                            currentTransitCar.DateRegister = dateRegister;
                            currentTransitCar.LastUpdate = DateTime.Now;
                        }
                        if (outOfDate != default(DateTime) && outOfDate != null)
                        {
                            currentTransitCar.OutOfDate = outOfDate;
                            currentTransitCar.LastUpdate = DateTime.Now;
                        }

                        _uow.TransitCars.update(currentTransitCar);
                        await _uow.SaveAsync();
                        return true;
                    }
                    else return false;
                }
                else return false;
            }return false;
        }

        public async Task<bool> DeleteTransitCarAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ifExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.TransitCarId == id && p.Status > 1 && p.IsActive);
                if(ifExist == null)
                {
                    var currentTransitCar = await _uow.TransitCars.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (currentTransitCar != null)
                    {
                        currentTransitCar.IsActive = false;
                    }

                    _uow.TransitCars.update(currentTransitCar);
                    await _uow.SaveAsync();
                    return true;
                }else return false;
               
            }
            else return false;
        }

        //public async Task<bool> UpdateIsWorkingAsync(string id)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        var ifExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.TransitCarId == id && p.IsDone && p.IsActive);
        //        if (ifExist == null)
        //        {
        //            var currentTransitCar = await _uow.TransitCars.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
        //            if (currentTransitCar != null)
        //            {
        //                currentTransitCar.IsWorking = !currentTransitCar.IsWorking;
        //            }

        //            _uow.TransitCars.update(currentTransitCar);
        //            await _uow.SaveAsync();
        //            return true;
        //        }
        //        else return false;

        //    }
        //    else return false;
        //}
    }
}
