using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.WorkingMappingRepositorys
{
    public interface IWorkingMappingRepository: IRepository<WorkMapping>
    {
        void update(WorkMapping workMapping);
    }
}
