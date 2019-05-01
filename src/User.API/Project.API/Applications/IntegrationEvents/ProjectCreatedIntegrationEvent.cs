using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.IntegrationEvents
{
    public class ProjectCreatedIntegrationEvent
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedTime { get; set; }

    }
}
