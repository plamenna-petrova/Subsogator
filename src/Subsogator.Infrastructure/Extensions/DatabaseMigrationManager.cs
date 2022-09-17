
using Data.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Subsogator.Infrastructure.Extensions
{
    public static class DatabaseMigrationManager
    {
        public static void MigrateDatabase<T>(this IApplicationBuilder applicationBuilder, ILogger<T> logger)
        {
            try
            {
                using (var serviceScope = applicationBuilder
                    .ApplicationServices.CreateScope())
                {
                    using (var applicationDbContext = serviceScope.ServiceProvider
                        .GetRequiredService<ApplicationDbContext>())
                    {
                        IEnumerable<string> pendingMigrations = applicationDbContext
                            .Database.GetPendingMigrations();

                        bool anyPendingMigrations = pendingMigrations.Any();

                        if (anyPendingMigrations)
                        {
                            applicationDbContext.Database.Migrate();
                            logger.LogInformation($"Pending migrations " +
                                $"applied to the database");
                        }
                        else
                        {
                            logger.LogInformation($"No pending migrations need " +
                                $"to be applied to the database");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogError($"Database Migration failed : {exception.Message}");
            }
        }
    }
}
