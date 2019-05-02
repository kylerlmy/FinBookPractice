using Recommend.API.Dtos;
using System.Threading.Tasks;

namespace Recommend.API.Service
{
    public interface IUserService
    {
        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserIdentity> GetBaseUseInfoAsync(int userId);
    }
}
