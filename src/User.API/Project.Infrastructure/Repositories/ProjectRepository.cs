using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;
using ProjectEntity = Project.Domain.AggregatesModel.Project;

namespace Project.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ProjectContext _projectContext;
        public ProjectRepository(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }
        public IUnitOfWork UnitOfWork => _projectContext;

        public ProjectEntity Add(ProjectEntity project)
        {
            if (project.IsTransient())
            {
                return _projectContext.Add(project).Entity;
            }
            return project;

        }

        public async Task<ProjectEntity> GetAsync(int id)
        {
            var project = await _projectContext.Projects
                  .Where(o => o.Id == id)
                  .Include(p => p.Properties)
                  .Include(p => p.Viewers)
                  .Include(p => p.Contributors)
                  .Include(p => p.VisibleRule)
                  .SingleOrDefaultAsync();

            return project;
        }

        public void Update(ProjectEntity project)
        {
            _projectContext.Update(project);
        }
    }
}
