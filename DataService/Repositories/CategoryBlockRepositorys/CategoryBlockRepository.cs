using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.CategoryBlockRepositorys
{
    public class CategoryBlockRepository : Repository<CategoryBlock>, ICategoryBlockRepository
    {
        private readonly TransferCompanyContext _context;
        public CategoryBlockRepository(TransferCompanyContext context):base(context)
        {
            _context = context;
        }

        public void update(CategoryBlock categoryBlock)
        {
            _context.Update(categoryBlock);
        }
    }
}
