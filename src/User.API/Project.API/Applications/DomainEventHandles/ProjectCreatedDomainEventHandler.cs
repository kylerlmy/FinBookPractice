using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project.Domain.Events;
using System.Threading;
using DotNetCore.CAP;
using Project.API.Applications.IntegrationEvents;

namespace Project.API.Applications.DomainEventHandles
{
    public class ProjectCreatedDomainEventHandler : INotificationHandler<ProjectCreatedEvent>
    {
        private ICapPublisher _capPublisher;
        public ProjectCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }
        public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectCreatedIntegrationEvent
            {
                ProjectId = notification.Project.Id,
                CreatedTime = DateTime.Now,
                UserId = notification.Project.UserId
            };

            await _capPublisher.PublishAsync("finbook.projectapi.projectcreated", @event, "ProjectCreated", cancellationToken);
        }
    }
}
