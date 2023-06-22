using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.TransitCarRepositorys
{
    public class TransitCarRepository : Repository<TransitCar>, ITransitCarRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public TransitCarRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _tempContext = tempContext;
        }
        public void update(TransitCar transitCar)
        {
            _tempContext.Update(transitCar);
        }
    }
}
