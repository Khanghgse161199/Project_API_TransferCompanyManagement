using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Entities;

namespace DataService.Repositories.OrderDetailRepositorys
{
    public interface IOrderShippingRepository: IRepository<OrderShipping>
    {
        void update(OrderShipping OrderDetail);
    }
}
