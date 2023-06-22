using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.OrderRepositorys
{
    public class MainOrderRepository : Repository<MainOrder>, IMainOrderRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public MainOrderRepository(TransferCompanyContext tempContext):base (tempContext)
        {
            _tempContext = tempContext;
        }

        public void update(MainOrder order)
        {
            _tempContext.Update(order);
        }
    }
}
