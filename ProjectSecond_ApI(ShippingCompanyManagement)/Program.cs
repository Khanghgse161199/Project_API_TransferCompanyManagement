using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DataService.Entities;
using DataService.HashService;
using DataService.Repositories.UnitOfWork;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Services.AccountServices;
using Services.AuthServices;
using Services.BlockServices;
using Services.CategoryBlockServices;
using Services.CategoryContainerservices;
using Services.ContainerServices;
using Services.EmployeeServices;
using Services.MappingService;
using Services.OrderDetailServices;
using Services.OrderSerivces;
using Services.RatingServices;
using Services.TokenServices;
using Services.TransitCarServices;
using Services.WorkingMappingSevices;

namespace ProjectSecond_ApI_ShippingCompanyManagement_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {
                builder.RegisterType<TransferCompanyContext>().AsSelf();
                builder.RegisterType<HashService>().As<IHashService>();
                builder.RegisterType<AccountService>().As<IAccountService>();
                builder.RegisterType<EmployeeService>().As<IEmployeeService>();
                builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
                builder.RegisterType<AuthService>().As<IAuthService>();
                builder.RegisterType<TokenService>().As<ITokenService>();
                builder.RegisterType<TransitCarService>().As<ITransitCarService>();
                builder.RegisterType<CategoryContainerService>().As<ICategoryContainerservice>();
                builder.RegisterType<ContainerService>().As<IContainerService>();
                builder.RegisterType<WorkingMappingService>().As<IWorkingMappingService>();
                builder.RegisterType<RatingService>().As<IRatingService>();
                builder.RegisterType<OrderShippingService>().As<IOrderDetailService>();
                builder.RegisterType<MainOrderService>().As<IMainOrderService>();
                builder.RegisterType<CategoryBlockService>().As<ICategoryBlockService>();
                builder.RegisterType<BlockServices>().As<IBlockServices>();
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }
            // turn on cross (WithOrigins("http://127.0.0.1:5500"))
            app.UseCors(t => t.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            // Configure the HTTP request pipeline.

            //app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}