using MediatR;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateProjectCommand, Domain.AggregatesModel.Project>
    {
        private IProjectRepository _projectRepository;

        public CreateOrderCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }
        public async Task<Domain.AggregatesModel.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            _projectRepository.Add(request.Project);

            await _projectRepository.UnitOfWork.SaveEntitiesAsync();

            return request.Project;
        }

    }
}
