using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectContributor:Entity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否是关闭者
        /// </summary>
        public bool IsClose { get; set; }

        /// <summary>
        /// 1：财务顾问 2：投资机构
        /// </summary>
        public int ContributorType { get; set; }
    }
}
