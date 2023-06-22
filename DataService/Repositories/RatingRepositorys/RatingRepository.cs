using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataService.Repositories.RatingRepositorys
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly TransferCompanyContext _tempContext;
        public RatingRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _tempContext = tempContext;
        }
        public void update(Rating rating)
        {
           _tempContext.Update(rating);
        }
    }
}
