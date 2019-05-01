using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class ViewProjectCommandHandler : AsyncRequestHandler<ViewProjectCommand>
    {
        private IProjectRepository _projectRepository;
        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }

        protected override async Task Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);

            if (project == null)
            {
                throw new ProjectDomainException($"project not found:{request.ProjectId}");
            }

            project.AddViewer(request.UserId, request.UserName, request.Avatar);

            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
