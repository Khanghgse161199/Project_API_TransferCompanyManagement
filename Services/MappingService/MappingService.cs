using AutoMapper;
using DataService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.Block;
using ViewModel.CategoryBlock;
using ViewModel.CategoryTran;
using ViewModel.EmployeeViewModel;
using ViewModel.Order;
using ViewModel.OrderDetail;
using ViewModel.Rating;
using ViewModel.TransitCar;
using ViewModel.WorkMapping;

namespace Services.MappingService
{
    public class MappingService : Profile
    {
        public MappingService() {
            CreateMap<Employee, EmployeeViewModel>();
            CreateMap<Employee, EmployeeProfileVIewModel>();
            CreateMap<TransitCar, TransitCarViewModel>();
            CreateMap<CategoryContainer, CategoryContainerViewModel>();
            CreateMap<WorkMapping, WorkMappingViewModel>();
            CreateMap<MainOrder, MainOrderViewModel>();
            CreateMap<CategoryBlock, CategoryBlockViewModel>();
            CreateMap<Block, BlockViewModel>();
            CreateMap<OrderShipping, OrderShippingViewModel>();
            CreateMap<Rating, RatingViewModel>();
        }
    }
}
