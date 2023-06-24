using AutoMapper;
using DataService.Entities;
using DataService.PageServices;
using DataService.Repositories.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Rating;

namespace Services.RatingServices
{
    public interface IRatingService
    {
        Task<bool> CreateRatingAsync(string id);
        Task<List<RatingViewModel>> GetAllRatingAsynsc();
        Task<RatingViewModel> GetRatingById(string id);
        Task<List<RatingViewModelWorkMapping>> GetRatingAboutEmployeeAsync(string id);
        Task<bool> UpdateRatingAsync(string id, double ratingPoint, string comment, string imgUrl, string reciver);
        Task<bool> DeleteRatingAsync(string id);
    }
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _uow;
        private readonly Mapper _Mapper;
        public RatingService(IUnitOfWork uow)
        {
            _uow = uow;
            var config = new MapperConfiguration(cfg =>
            {

                cfg.AddProfile<MappingService.MappingService>();
            });
            _Mapper = new Mapper(config);
        }

        public async Task<bool> CreateRatingAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var newRating = new Rating()
                {
                    Id = id, 
                    Reciver = null,
                    Comment = null,
                    ImgUrl = null,
                    IsActive = true,    
                    RatingPoint = 0,
                    DateTimeCreate = DateTime.Now,
                    LastUpdate = null
                };

                await _uow.Ratings.AddAsync(newRating);
                await _uow.SaveAsync();
                return true;
            }
            else return false;
        }

        public async Task<List<RatingViewModel>> GetAllRatingAsynsc()
        {
            var ratings = await _uow.Ratings.GetAllAsync(p =>  !string.IsNullOrEmpty(p.Reciver) && p.RatingPoint != null && p.IsActive);
            if (ratings.Count > 0)
            {
                return _Mapper.Map<List<RatingViewModel>>(ratings); 
            }
            else return null;
        }

        public async Task<RatingViewModel> GetRatingById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var currentRating = await _uow.Ratings.FirstOfDefaultAsync(p => p.Id == id && !string.IsNullOrEmpty(p.Reciver) && p.RatingPoint != null  && p.IsActive);
                if (currentRating != null)
                {
                    return _Mapper.Map<RatingViewModel>(currentRating);
                }
                else return null;
            }
            else return null;
        }

        public async Task<List<RatingViewModelWorkMapping>> GetRatingAboutEmployeeAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var CurrentEmployee = await _uow.Employees.FirstOfDefaultAsync(p => p.Id == id, "WorkMappings,Acc");
                if(CurrentEmployee != null)
                {
                    if(CurrentEmployee.Acc.IsActive)
                    {
                        List <RatingViewModelWorkMapping> tmp = new List <RatingViewModelWorkMapping>();
                        var workingMappings = await _uow.WorkingMappings.GetAllAsync(p => p.EmployeeId == CurrentEmployee.Id && p.IsActive, null, "Rating");
                        if (workingMappings != null)
                        {
                            foreach (var item in workingMappings)
                            {
                                if (item.Rating.RatingPoint > 0 && item.Rating.IsActive && item.Rating.Reciver != null)
                                {
                                    tmp.Add(new RatingViewModelWorkMapping
                                    {
                                        Id = item.Rating.Id,
                                        RatingPoint = item.Rating.RatingPoint,
                                        Comment = item.Rating.Comment,
                                        ImgUrl = item.Rating.ImgUrl,
                                        Reciver = item.Rating.Reciver,
                                        DateTimeCreate = item.Rating.DateTimeCreate,
                                    });
                                }
                            }
                            var tmps = tmp.AsQueryable();
                            if (tmps.Count() > 0)
                            {
                                tmps = tmps.OrderByDescending(x => x.DateTimeCreate);
                                var newTmps = PaginateList<RatingViewModelWorkMapping>.CreatePaginateList(tmps, 1, 1);
                                return newTmps.ToList();
                            }
                            else return null;
                          
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else return null;
                }
                else return null;
            }
            else return null;
        }

        public async Task<bool> UpdateRatingAsync(string id, double ratingPoint, string comment, string imgUrl, string reciver)
        {
            if (!string.IsNullOrEmpty(id) && ratingPoint >= 1 && ratingPoint <= 5) {
                var ifExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.RatingId == id && p.Status == 3 && p.IsActive);
                if(ifExist != null) {
                    var currentRating = await _uow.Ratings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
                    if (!string.IsNullOrEmpty(comment))
                    {
                        currentRating.Comment = comment;
                        currentRating.LastUpdate = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(imgUrl))
                    {
                        currentRating.ImgUrl = imgUrl;
                        currentRating.LastUpdate = DateTime.Now;
                    }
                    if (!string.IsNullOrEmpty(reciver))
                    {
                        currentRating.Reciver = reciver;
                        currentRating.LastUpdate = DateTime.Now;
                    }
                    if (ratingPoint != 0)
                    {
                        currentRating.RatingPoint = ratingPoint;
                        currentRating.LastUpdate = DateTime.Now;
                    }
                    else return false;

                    _uow.Ratings.update(currentRating);
                    await _uow.SaveAsync();
                    var isUpdate = await UpdateSummaryRatingWrokingMapping(id);
                    if (isUpdate)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        public async Task<bool> DeleteRatingAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var ifExist = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.RatingId == id && p.Status == 3 && p.IsActive);
                if (ifExist != null)
                {
                    var currentRating = await _uow.Ratings.FirstOfDefaultAsync(p => p.Id == id && !string.IsNullOrEmpty(p.Reciver) && p.RatingPoint != null && p.IsActive);
                    if (currentRating != null)
                    {
                        currentRating.IsActive = false;
                        _uow.Ratings.update(currentRating);
                        await _uow.SaveAsync();
                        var isUpdate = await UpdateSummaryRatingWrokingMapping(currentRating.Id);
                        if (isUpdate)
                        {
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        private async Task<bool> UpdateSummaryRatingWrokingMapping(string id)
        {
            var currentWorkingMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.RatingId == id && p.Status == 3, "Employee");
            if (currentWorkingMapping != null)
            {
                var WorkingMappings = await _uow.WorkingMappings.GetAllAsync(p => p.EmployeeId == currentWorkingMapping.EmployeeId && p.IsActive && p.Status == 3, null, "Rating");
                if (WorkingMappings.Count > 0)
                {
                    float countOne = 0;
                    float countTwo = 0;
                    float countThree = 0;
                    float countFour = 0;
                    float countFive = 0;
                    float count = 0;
                    foreach (var item in WorkingMappings)
                    {
                        if(item.Rating.RatingPoint > 0 && item.Rating.IsActive)
                        {
                            if (item.Rating.RatingPoint == 1)
                            {
                                countOne++;
                            }
                            if (item.Rating.RatingPoint == 2)
                            {
                                countTwo++;
                            }
                            if (item.Rating.RatingPoint == 3)
                            {
                                countThree++;
                            }
                            if (item.Rating.RatingPoint == 4)
                            {
                                countFour++;
                            }
                            if (item.Rating.RatingPoint == 5)
                            {
                                countFive++;
                            }
                            count++;
                        }
                    }
                    float summaryRating = (((countOne * 1) + (countTwo * 2) + (countThree * 3) + (countFour * 4) + (countFive * 5)) / (float)count);
                    currentWorkingMapping.Employee.SummayRating = summaryRating;
                    _uow.Employees.Udpate(currentWorkingMapping.Employee);
                    await _uow.SaveAsync();
                    return true;
                }
                else return false;
            }
            else return false;
        }
        //private async Task<bool> UpdateSummaryRatingEmloyee(string id)
        //{
        //    var currentWorkMapping = await _uow.WorkingMappings.FirstOfDefaultAsync(p => p.Id == id && p.IsActive);
        //    if(currentWorkMapping != null)
        //    {
        //        int count = 0;
        //        double totalRatingSummary = 0;
        //        var currentEmployee = await _uow.Employees.FirstOfDefaultAsync(p => p.Id == currentWorkMapping.EmployeeId, "WorkMappings");
        //        if (currentEmployee.WorkMappings.Count > 0)
        //        {
        //            foreach (var item in (currentEmployee.WorkMappings))
        //            {
        //                if(item.SummaryRating > 0)
        //                {
        //                    totalRatingSummary = totalRatingSummary + item.SummaryRating;
        //                    count++;
        //                }
        //            }
        //            currentEmployee.SummayRating = totalRatingSummary/(double)count;
        //            _uow.Employees.Udpate(currentEmployee);
        //            await _uow.SaveAsync();
        //            return true;
        //        }
        //        else return false;
        //    }else return false;
        //}
    }
}
