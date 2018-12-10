using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Dtos
{
    public class UserIdentity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public string Company { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

    }
}
