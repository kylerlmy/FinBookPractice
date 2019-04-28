using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectViewer:Entity
    {

        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
