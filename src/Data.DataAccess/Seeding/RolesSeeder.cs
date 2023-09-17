using Data.DataModels.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Subsogator.Common.GlobalConstants;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    public class RolesSeeder : ISeeder
    {
        public async Task<bool> SeedDatabase(
            ApplicationDbContext applicationDbContext, 
            IServiceProvider serviceProvider
        )
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            bool isRolesSeedingTriggered = false;

            string[] roleNames = {
                IdentityConstants.AdministratorRoleName, 
                IdentityConstants.EditorRoleName, 
                IdentityConstants.TranslatorRoleName,
                IdentityConstants.UploaderRoleName,
                IdentityConstants.NormalUserRole 
            };

            int createdRolesCounter = 0;

            foreach (var roleName in roleNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null)
                {
                    if (!await SeedRoleAsync(applicationDbContext, roleManager, roleName))
                    {
                        break;
                    }
                    else
                    {
                        createdRolesCounter++;
                    }
                }
            }

            if (createdRolesCounter > 0)
            {
                isRolesSeedingTriggered = true;
            }

            return isRolesSeedingTriggered;
        }

        public static async Task<bool> SeedRoleAsync(
            ApplicationDbContext applicationDbContext, 
            RoleManager<ApplicationRole> roleManager, string roleName
        )
        {
            bool isRoleCreated = true;

            var newApplicationRole = new ApplicationRole(roleName);

            applicationDbContext.Entry(newApplicationRole).State = EntityState.Added;

            var roleCreationResult = await roleManager.CreateAsync(newApplicationRole);

            await applicationDbContext.SaveChangesAsync();

            if (!roleCreationResult.Succeeded)
            {
                isRoleCreated = false;

                throw new Exception(string.Join(
                    Environment.NewLine, 
                    roleCreationResult.Errors.Select(e => e.Description)
                ));
            }

            return isRoleCreated;
        }
    }
}
