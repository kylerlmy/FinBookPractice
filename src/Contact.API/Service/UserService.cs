using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.API.Dtos;

namespace Contact.API.Service
{
    public class UserService:IUserService
    {
        public Task<BaseUseInfo> GetBaseUseInfoAsync(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
