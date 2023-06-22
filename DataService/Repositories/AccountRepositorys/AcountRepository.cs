using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.AccountRepository
{
    public class AcountRepository : Repository<Account>, IAccountRepository
    {
        private readonly TransferCompanyContext _context;
        public AcountRepository(TransferCompanyContext tempContext):base(tempContext)
        {
            _context = tempContext;
        }
     
        public void Udpate(Account account)
        {
            _context.Update(account);
        }

    }
}
