using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.ContainerRepositorys
{
    public class ContainerRepository : Repository<Entities.Container>, IContainerRepository
    {
        private readonly TransferCompanyContext _context;
        public ContainerRepository(TransferCompanyContext context):base(context)
        {
            _context = context;
        }

        public void update(Entities.Container container)
        {
           _context.Update(container);
        }
    }
}
