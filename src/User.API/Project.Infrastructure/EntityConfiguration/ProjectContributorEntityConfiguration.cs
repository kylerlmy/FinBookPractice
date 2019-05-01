using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectContributorEntityConfiguration : IEntityTypeConfiguration<ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
            builder.ToTable("ProjectContributors")
                 .HasKey(p => p.Id);
        }
    }
}
