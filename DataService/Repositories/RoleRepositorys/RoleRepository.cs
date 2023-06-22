using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.RoleRepositorys
{
    public class RoleRepository: Repository<Role>, IRoleRepository
    {
        private readonly TransferCompanyContext _context;

        public RoleRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _context = tempContext;
        }

        public void Udpate(Role role)
        {
            _context.Update(role);
        }


    }
}
