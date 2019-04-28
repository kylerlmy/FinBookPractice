using Microsoft.EntityFrameworkCore;
using Project.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Project.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
