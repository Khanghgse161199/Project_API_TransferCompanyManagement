using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.RoleRepositorys
{
    public interface IRoleRepository : IRepository<Role>
    {
        void Udpate(Role role);
    }
}
