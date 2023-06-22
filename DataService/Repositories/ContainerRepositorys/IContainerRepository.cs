using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using DataService.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.ContainerRepositorys
{
    public interface IContainerRepository : IRepository<Entities.Container>
    {
        void update (Entities.Container container);
    }
}
