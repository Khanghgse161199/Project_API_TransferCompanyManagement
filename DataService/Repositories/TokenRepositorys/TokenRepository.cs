using DataService.Entities;
using DataService.Repositories.Repository;
using DataService.Repositories.TokenRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.TokenRepository
{
    public class TokenRepository: Repository<Token>, ITokenRepository
    {
        private readonly TransferCompanyContext _context;

        public TokenRepository(TransferCompanyContext tempContext) : base(tempContext)
        {
            _context = tempContext;
        }
        public void Udpate(Token token)
        {
            _context.Update(token);
        }
    }
}
