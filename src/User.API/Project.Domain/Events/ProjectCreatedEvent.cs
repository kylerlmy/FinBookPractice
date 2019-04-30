using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Events
{
    public class ProjectCreatedEvent : INotification
    {
        public AggregatesModel.Project Project { get; set; }
    }
}
