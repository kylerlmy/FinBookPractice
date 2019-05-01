using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.API.Applications.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly string _connectionString;

        public ProjectQueries(string connectString)
        {
            _connectionString = connectString;
        }

        public async Task<dynamic> GetProjectByUserId(int userId)
        {
            var sqlString = @"SELECT 
                                Projects.Id,
                                Projects.Avatar,
                                Projects.Company,
                                Projects.FinStage,
                                Projects.Introduction,
                                Projects.ShowSecurityInfo,
                                Projects.CreateTime
                                FROM Projects
                                WHERE Projects.UserId=@userId";

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>(sqlString, new { userId });

                return result;
            }

        }

        public async Task<dynamic> GetProjectDetail(int projectId)
        {
            var sqlString = @"SELECT 
                                Projects.Company,
                                Projects.City,
                                Projects.ArcaName
                                Projects.Province
                                Projects.FinStage,
                                Projects.FinMoney,
                                Projects.Valuation,
                                Projects.FinPercentage,
                                Projects.Introduction,
                                Projects.UserId,
                                Projects.Income,
                                Projects.Revenue,
                                Projects.UserName,
                                Projects.Avatar,
                                Projects.BrokerageOptions,
                                ProjectVisibleRules.Tags
                                ProjectVisibleRules.Visible,
                                FROM
                                Projects INNER JOIN ProjectVisibleRules
                                ON Projects.Id=ProjectVisibleRules.ProjectId
                                WHERE Projects.Id=@projectId ";
                                //AND Projects.UserId=@userId";


            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.QueryAsync<dynamic>(sqlString, new { projectId });

                return result;
            }
        }
    }
}
