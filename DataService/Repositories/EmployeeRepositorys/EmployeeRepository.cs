using DataService.Entities;
using DataService.Repositories.AccountRepository;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.EmployeeRepository
{
    // Repository<Employee> trong trường hợp này kế thừa trước những method của IRepository
    public class EmployeeRepository: Repository<Employee>, IEmployeeRepository
    {
        private readonly TransferCompanyContext _context;
        public EmployeeRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _context = tempContext;
        }

        public void Udpate(Employee employee)
        {
            _context.Update(employee);
        }
    }
}
