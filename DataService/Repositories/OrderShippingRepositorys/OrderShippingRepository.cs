using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.OrderDetailRepositorys
{
    public class OrderShippingRepository : Repository<OrderShipping>, IOrderShippingRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public OrderShippingRepository(TransferCompanyContext tempContext):base(tempContext)
        {
            _tempContext = tempContext;
        }

        public void update(OrderShipping OrderDetail)
        {
            _tempContext.Update(OrderDetail);
        }
    }
}
