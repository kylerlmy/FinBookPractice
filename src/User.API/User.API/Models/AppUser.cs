using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Models
{
    public class AppUser//如果，User作为聚合根，那么，BPFile和UserTag就需要依托于User的基础上，进行持久化。包括更新操作和查询操作也是如此。聚合根，产生的问题时，导致模型或数据的操作方式过于僵化。例如，如果User为聚合根，那么User就为当前领域的访问的入口，BPFile和UserTag就需要依托于User，这种情况就操作，如果单独查询UserTag，是不被允许的，必须要把最大的根User给拿出来，然后再去查UserTag。所以这种DDD的设计需要根据设计的需要进行取舍。再使用CQRS进行读写分离时，查询的业务就不属于这块了（DDD？），随便写SQL是不相关的。在实现项目的时候，会主要使用DDD
    {

        public AppUser()
        {
            Properties = new List<UserProperty>();
        }
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 性别 1：男 0：女
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 省Id
        /// </summary>
        public string ProvinceId { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 名片地址
        /// </summary>
        public string NameCard { get; set; }

        /// <summary>
        /// 用户属性列表
        /// </summary>
        public List<UserProperty> Properties { get; set; }//属于用户的一部分，不会单独的去查询


    }
}
