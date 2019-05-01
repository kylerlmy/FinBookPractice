using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectViewerEntityConfiguration : IEntityTypeConfiguration<ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<ProjectViewer> builder)
        {
            builder.ToTable("ProjectViewers")
                  .HasKey(p => p.Id);
        }
    }
}
