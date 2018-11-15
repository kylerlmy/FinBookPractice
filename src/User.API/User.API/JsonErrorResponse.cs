using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API
{
    public class JsonErrorResponse
    {
        public string Message { get; set; }

        /// <summary>
        /// 开发环境显示，生产环境不显示
        /// </summary>
        public object DeveloprMessage { get; set; }
    }
}
