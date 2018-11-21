using Contact.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.API.Data
{
    public interface IContactRepository
    {
        //Mongo查询和更新是一起完成的


        /// <summary>
        /// 更新联系人信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        Task<bool> UpdateContactInfo(BaseUseInfo userinfo, CancellationToken cancellationToken);
    }
}
