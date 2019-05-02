using Microsoft.EntityFrameworkCore;
using Recommend.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recommend.API.Data
{
    public class RecommendDbContext : DbContext
    {
        public RecommendDbContext(DbContextOptions<RecommendDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectRecommend>().ToTable("ProjectRecommends")
                .HasKey(p => p.Id);
            base.OnModelCreating(modelBuilder);
        }

    }
}
