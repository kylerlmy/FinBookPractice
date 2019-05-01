using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.AggregatesModel
{
    public interface IProjectRepository : IRepository<Project>
    {

        Project Add(Project project);
        void Update(Project project);
        Task<Project> GetAsync(int id);
    }
}
