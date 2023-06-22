using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.CategoryTransRepositorys
{
    public class CategoryContainerRepository : Repository<CategoryContainer>, ICategoryContainerRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public CategoryContainerRepository(TransferCompanyContext tempContext):base(tempContext)
        {
            _tempContext = tempContext;
        }

        public void update(CategoryContainer categoryTran)
        {
            _tempContext.Update(categoryTran);
        }
    }
}
