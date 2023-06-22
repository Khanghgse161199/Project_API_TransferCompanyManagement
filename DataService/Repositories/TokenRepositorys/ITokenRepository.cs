using DataService.Entities;
using DataService.Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repositories.TokenRepositorys
{
    public interface ITokenRepository: IRepository<Token>
    {
        void Udpate(Token token);
    }
}
