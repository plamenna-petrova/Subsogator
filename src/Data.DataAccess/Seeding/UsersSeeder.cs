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
    public class UsersSeeder : ISeeder
    {
        public async Task<bool> SeedDatabase(
            ApplicationDbContext applicationDbContext, 
            IServiceProvider serviceProvider
        )
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var databaseLoadedUsers = applicationDbContext.Users.IgnoreQueryFilters().ToList();

            bool isUsersSeedingTriggered = false;

            string[] usernames =
            {
                IdentityConstants.AdministratorUsername,
                IdentityConstants.EditorUsername,
                IdentityConstants.UploaderUsername,
                IdentityConstants.ExampleUserUsername
            };

            string[] emails = {
                IdentityConstants.AdministratorEmail,
                IdentityConstants.EditorEmail,
                IdentityConstants.UploaderEmail,
                IdentityConstants.ExampleUserEmail
            };

            string[] passwords =
            {
                IdentityConstants.AdministratorPassword,
                IdentityConstants.EditorPassword,
                IdentityConstants.UploaderPassword,
                IdentityConstants.ExampleUserPassword
            };

            string[] roles =
            {
                IdentityConstants.AdministratorRoleName,
                IdentityConstants.EditorRoleName,
                IdentityConstants.UploaderRoleName,
                IdentityConstants.NormalUserRole
            };

            int usersToSeedCount = usernames.Length;

            int createdUsersCounter = 0;

            for (int i = 0; i < usersToSeedCount; i++)
            {
                var user = databaseLoadedUsers
                    .FirstOrDefault(user => user.Email == emails[i] || user.UserName == usernames[i]);

                if (user == null)
                {
                    if (!await SeedUserAsync(userManager, usernames[i], emails[i], passwords[i], roles[i]))
                    {
                        break;
                    }
                    else
                    {
                        createdUsersCounter++;
                    }
                }
            }

            if (createdUsersCounter > 0)
            {
                isUsersSeedingTriggered = true;
            }

            return isUsersSeedingTriggered;
        }

        public static async Task<bool> SeedUserAsync(
            UserManager<ApplicationUser> userManager,
            string username, string email, string password, string role
        )
        {
            bool isUserCreated = true;

            ApplicationUser userToRegister = new ApplicationUser()
            {
                Email = email,
                UserName = username,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(userToRegister, password);

            var addUserToRoleResult = await userManager.AddToRoleAsync(userToRegister, role);

            if (!result.Succeeded || !addUserToRoleResult.Succeeded)
            {
                isUserCreated = false;

                throw new Exception(string.Join(
                    Environment.NewLine, 
                    result.Errors.Select(e => e.Description)
                ));
            }

            return isUserCreated;
        }
    }
}
