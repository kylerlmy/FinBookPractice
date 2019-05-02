using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Dtos
{
    public class Contact
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 好友 Id
        /// </summary>
        public int ContactId { get; set; }
    }
}
