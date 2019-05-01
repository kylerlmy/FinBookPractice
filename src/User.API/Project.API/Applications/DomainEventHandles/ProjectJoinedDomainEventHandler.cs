using DotNetCore.CAP;
using MediatR;
using Project.API.Applications.IntegrationEvents;
using Project.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Applications.DomainEventHandles
{
    public class ProjectJoinedDomainEventHandler : INotificationHandler<ProjectJoinedEvent>
    {

        private ICapPublisher _capPublisher;
        public ProjectJoinedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(ProjectJoinedEvent notification, CancellationToken cancellationToken)
        {
            var @event = new ProjectJoinedIntegrationEvent
            {
                Avatar = notification.Avatar,
                Company = notification.Company,
                Contributor = notification.Contributor,
                Introduction = notification.Introduction
            };

            await _capPublisher.PublishAsync("finbook.projectapi.projectjoined", @event, "ProjectJoined", cancellationToken);
        }
    }
}
