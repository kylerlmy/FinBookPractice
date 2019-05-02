﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recommend.API.Data;

namespace Recommend.API.Migrations
{
    [DbContext(typeof(RecommendDbContext))]
    partial class RecommendDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Recommend.API.Models.ProjectRecommend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("FinState");

                    b.Property<string>("FromUserAvatar");

                    b.Property<int>("FromUserId");

                    b.Property<string>("FromUserName");

                    b.Property<string>("Introduction");

                    b.Property<string>("ProjectAvatar");

                    b.Property<int>("ProjectId");

                    b.Property<DateTime>("RecommendTime");

                    b.Property<int>("RecommendType");

                    b.Property<string>("Tags");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ProjectRecommends");
                });
#pragma warning restore 612, 618
        }
    }
}
