using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.API.Models
{
    public class Contact
    {
        public Contact()
        {
            Tags = new List<string>();
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string   Avatar { get; set; }

        public List<string> Tags { get; set; }
    }
}
