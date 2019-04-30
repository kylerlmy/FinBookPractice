using MediatR;
using Project.Domain.AggregatesModel;
using Project.Domain.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Project.API.Applications.Commands
{
    public class JoinProjectCommandHandler : AsyncRequestHandler<JoinProjectCommand>
    {
        private IProjectRepository _projectRepository;
        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

        }


        protected override async Task Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.Contributor.ProjectId);

            if (project == null)
            {
                throw new ProjectDomainException($"project not found:{request.Contributor.ProjectId}");
            }

            project.AddContributor(request.Contributor);

            await _projectRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
