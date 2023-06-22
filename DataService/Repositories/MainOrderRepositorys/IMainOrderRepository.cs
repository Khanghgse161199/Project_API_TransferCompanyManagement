using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.OrderRepositorys
{
    public interface IMainOrderRepository: IRepository<MainOrder>
    {
        void update(MainOrder order);
    }
}
