using Project.Domain.Events;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class Project : Entity, IAggregateRoot
    {

        public Project()
        {
            Viewers = new List<ProjectViewer>();
            Contributors = new List<ProjectContributor>();

            AddDomainEvent(new ProjectCreatedEvent { Project = this });
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 项目Logo
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// 原BP文件地址
        /// </summary>
        public string OriginBPFile { get; set; }

        /// <summary>
        /// 转换后的BP文件地址
        /// </summary>
        public string FormateBPFile { get; set; }

        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }

        /// <summary>
        /// 公司所在省的ID
        /// </summary>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 公司所在省的名称
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 公司所在城市Id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 公司所在城市名称
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 公司所在区域Id
        /// </summary>
        public int ArcaId { get; set; }

        /// <summary>
        /// 公司所在区域名称
        /// </summary>
        public string ArcaName { get; set; }

        /// <summary>
        /// 公司成立时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 项目基本信息
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 出让股份比例
        /// </summary>
        public string FinPercentage { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

        /// <summary>
        /// 融资金额 单位（万）
        /// </summary>
        public int FinMoney { get; set; }

        /// <summary>
        /// 收入 单位（万）
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 利润 单位（万）
        /// </summary>
        public int Revenue { get; set; }

        /// <summary>
        /// 估值 单位（万）
        /// </summary>
        public int Valuation { get; set; }

        /// <summary>
        /// 佣金分配方式
        /// </summary>
        public int BrokerageOptions { get; set; }

        /// <summary>
        /// 是否委托给 finbook
        /// </summary>
        public bool OnPlatform { get; set; }

        /// <summary>
        /// 可见范围设置
        /// </summary>
        public ProjectVisibleRule VisibleRule { get; set; }

        /// <summary>
        /// 根引用项目Id
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 上级引用项目Id
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 项目属性：行业领域 融资币种
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }

        /// <summary>
        /// 贡献者列表
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }

        /// <summary>
        /// 查看者
        /// </summary>
        public List<ProjectViewer> Viewers { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        public DateTime CreateTime { get; set; }

        private Project CloneProject(Project source = null)
        {
            if (source == null)
                source = this;

            var newProject = new Project
            {
                ArcaId = source.ArcaId,
                ArcaName = source.ArcaName,
                Avatar = source.Avatar,
                BrokerageOptions = source.BrokerageOptions,
                City = source.City,
                CityId = source.CityId,
                Company = source.Company,
                Contributors = new List<ProjectContributor> { },
                CreateTime = DateTime.Now,
                FinMoney = source.FinMoney,
                FinPercentage = source.FinPercentage,
                FinStage = source.FinStage,
                FormateBPFile = source.FormateBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OnPlatform = source.OnPlatform,
                OriginBPFile = source.OriginBPFile,
                Province = source.Province,
                ProvinceId = source.ProvinceId,
                ReferenceId = source.ReferenceId,
                RegisterTime = source.RegisterTime,
                Revenue = source.Revenue,
                ShowSecurityInfo = source.ShowSecurityInfo,
                SourceId = source.SourceId,
                Tags = source.Tags,
                UpdateTime = source.UpdateTime,
                UserId = source.UserId,
                Valuation = source.Valuation,
                Viewers = new List<ProjectViewer> { },
                VisibleRule = source.VisibleRule == null ? null : new ProjectVisibleRule
                {
                    Visible = source.VisibleRule.Visible,
                    Tags = source.VisibleRule.Tags
                }
            };

            newProject.Properties = new List<ProjectProperty> { };

            foreach (var item in source.Properties)
            {
                newProject.Properties.Add(new ProjectProperty(item.Key, item.Text, item.Value));
            }
            return newProject;
        }


        /// <summary>
        /// 参与者得到项目拷贝
        /// </summary>
        /// <param name="contributorId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public Project ContributorFork(int contributorId, Project source = null)
        {
            if (source == null)
                source = this;

            var newProject = CloneProject(source);

            newProject.UserId = contributorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            newProject.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProject.UpdateTime = DateTime.Now;

            return newProject;
        }


        public void AddViewer(int userId, string userName, string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreateTime = DateTime.Now
            };


            if (!Viewers.Any(v => v.UserId == userId))
            {
                Viewers.Add(viewer);
                AddDomainEvent(new ProjectViewedEvent { Viewer = viewer, Company = Company, Introduction = Introduction });
            }

        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == contributor.UserId))
            {
                Contributors.Add(contributor);

                AddDomainEvent(new ProjectJoinedEvent { Contributor = contributor, Avatar = Avatar, Company = Company, Introduction = Introduction });
            }
        }



    }
}
