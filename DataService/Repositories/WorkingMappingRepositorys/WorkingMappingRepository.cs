using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.WorkingMappingRepositorys
{
    public class WorkingMappingRepository : Repository<WorkMapping>, IWorkingMappingRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public WorkingMappingRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _tempContext = tempContext;
        }
        public void update(WorkMapping workMapping)
        {
            _tempContext.Update(workMapping);
        }
    }
}
