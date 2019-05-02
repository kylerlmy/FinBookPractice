using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.IntegrationEvents
{
    public class ProjectCreatedIntegrationEvent
    {

        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectAvatar { get; set; }
        public string Company { get; set; }
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string Introduction { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinState { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
