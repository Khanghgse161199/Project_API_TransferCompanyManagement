using DataService.Entities;
using DataService.Repositories.AccountRepository;
using DataService.Repositories.BlockRepositorys;
using DataService.Repositories.CategoryBlockRepositorys;
using DataService.Repositories.CategoryTransRepositorys;
using DataService.Repositories.ContainerRepositorys;
using DataService.Repositories.EmployeeRepository;
using DataService.Repositories.OrderDetailRepositorys;
using DataService.Repositories.OrderRepositorys;
using DataService.Repositories.RatingRepositorys;
using DataService.Repositories.RoleRepositorys;
using DataService.Repositories.TokenRepositorys;
using DataService.Repositories.TransitCarRepositorys;
using DataService.Repositories.WorkingMappingRepositorys;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
        IAccountRepository Accounts
        { 
            get; 
        }
        IEmployeeRepository Employees
        {
            get;
        }
        IRoleRepository Roles
        {
            get;
        }
        ITokenRepository Tokens
        {
            get;
        }
        IBlockRepository Blocks { get; }
        ICategoryBlockRepository CategoryBlocks { get; }
        ICategoryContainerRepository CategoryContainers { get; }
        IContainerRepository Containers { get; }
        IOrderShippingRepository OrderShippings { get; }
        IMainOrderRepository MainOrders { get; }
        IRatingRepository Ratings { get; }
        ITransitCarRepository TransitCars { get; }
        IWorkingMappingRepository WorkingMappings { get; }
    }
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DataService.Entities.TransferCompanyContext _db;
        public IAccountRepository Accounts { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public ITokenRepository Tokens { get; private set; }
        public IBlockRepository Blocks { get; private set; }
        public ICategoryBlockRepository CategoryBlocks { get; private set; }
        public ICategoryContainerRepository CategoryContainers { get; private set; }
        public IContainerRepository Containers { get; private set; }
        public IOrderShippingRepository OrderShippings { get; private set; }
        public IMainOrderRepository MainOrders { get; private set; }
        public IRatingRepository Ratings { get; private set; }
        public ITransitCarRepository TransitCars { get; private set; }
        public IWorkingMappingRepository WorkingMappings { get; private set; }

        public UnitOfWork(TransferCompanyContext db)
        {
            _db = db;
            Accounts = new AcountRepository(_db);
            Employees = new EmployeeRepository.EmployeeRepository(_db);
            Roles = new RoleRepository(_db);
            Tokens = new TokenRepository.TokenRepository(_db);
            Blocks = new BlockRepository(_db);
            CategoryBlocks = new CategoryBlockRepository(_db);
            CategoryContainers = new CategoryContainerRepository(_db);
            Containers = new ContainerRepository(_db);
            OrderShippings = new OrderShippingRepository(_db);
            MainOrders = new MainOrderRepository(_db);
            Ratings = new RatingRepository(_db);
            TransitCars = new TransitCarRepository(_db);
            WorkingMappings = new WorkingMappingRepository(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
