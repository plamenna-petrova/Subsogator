
using Data.DataAccess;
using Data.DataAccess.Seeding;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Subsogator.Infrastructure.Extensions
{
    public static class ApplicationDbContextSeeder
    {
        public static void ApplyDatabaseSeeding<T>(this IApplicationBuilder app, ILogger<T> logger)
        {
            try
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    using (var applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                    {
                        var seeders = new List<ISeeder>
                        {
                            new EntitiesSeeder()
                        };

                        foreach (var seeder in seeders)
                        {
                            seeder.SeedDatabase(applicationDbContext);
                            logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception.Message);
            }
        }
    }
}
