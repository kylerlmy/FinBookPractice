using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.API.Data
{
    public class UserContextSeed
    {
        private ILogger<UserContextSeed> _logger;

        public UserContextSeed(ILogger<UserContextSeed> logger)
        {
            _logger = logger;
        }


        public static async Task SeedAsync(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, int? retry = 0)
        {
            var retryForAvaiabliblity = retry.Value;

            try
            {
                using (var scope = applicationBuilder.ApplicationServices.CreateScope())
                {
                    var context = (UserContext)scope.ServiceProvider.GetService(typeof(UserContext));
                    var logger = (ILogger<UserContextSeed>)scope.ServiceProvider.GetService(typeof(ILogger<UserContextSeed>));

                    logger.LogDebug("Begin UserContextSeed SeedAsync");

                    context.Database.Migrate();

                    if (!context.Users.Any())
                    {
                        context.Users.Add(new Models.AppUser { Name = "kyle" });
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {

                if (retryForAvaiabliblity < 10)
                {
                    retryForAvaiabliblity++;
                    var logger = loggerFactory.CreateLogger(typeof(UserContext));
                    logger.LogError(ex.Message);

                    await SeedAsync(applicationBuilder, loggerFactory, retryForAvaiabliblity);
                }
            }

        }

    }
}
