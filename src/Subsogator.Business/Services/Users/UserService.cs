using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Data.DataModels.Entities.Identity;
using Data.DataModels.Enums;
using Microsoft.AspNetCore.Identity;
using Subsogator.Web.Models.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Subsogator.Common.GlobalConstants.IdentityConstants;

namespace Subsogator.Business.Services.Users
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IEnumerable<AllUsersViewModel> GetAllUsers()
        {
            var allUsers = _userRepository
                  .GetAllAsNoTracking()
                    .Select(u => new AllUsersViewModel
                    {
                        Id = u.Id,
                        Username = u.UserName,
                        PromotionStatus = u.PromotionStatus,
                        PromotionLevel = u.PromotionLevel,
                        Roles = u.ApplicationUserRoles.Select(aur => aur.Role.Name).ToList()
                    });

            return allUsers;
        }

        public async Task PromoteUser(string userId)
        {
            var user = FindUser(userId);

            if (user.PromotionStatus == PromotionStatus.Pending)
            {
                var roles = _userManager.GetRolesAsync(user).Result.ToArray();

                if (roles.Length == 1)
                {
                    var userRole = roles[0];

                    switch (user.PromotionLevel)
                    {
                        case "Uploader":
                            if (userRole == NormalUserRole)
                            {
                                await AssignRole(user, NormalUserRole, UploaderRoleName);

                                user.PromotionStatus = PromotionStatus.Accepted;
                                user.PromotionLevel = UploaderRoleName;
                            }
                            break;
                        case "Editor":
                            if (userRole == UploaderRoleName)
                            {
                                await AssignRole(user, UploaderRoleName, EditorRoleName);

                                user.PromotionStatus = PromotionStatus.Accepted;
                                user.PromotionLevel = EditorRoleName;
                            }
                            break;
                    }

                    _userRepository.Update(user);
                }
            }
        }

        public void DeclinePromotion(string userId)
        {
            var user = FindUser(userId);

            if (user.PromotionStatus == PromotionStatus.Pending)
            {
                user.PromotionStatus = PromotionStatus.Declined;
                user.PromotionLevel = null;
            }

            _userRepository.Update(user);
        }

        public void EnrollForUploaderRole(string userId)
        {
            var user = FindUser(userId);

            user.PromotionStatus = PromotionStatus.Pending;
            user.PromotionLevel = "Uploader";

            _userRepository.Update(user);
        }

        public ApplicationUser FindUser(string userId)
        {
            return _userRepository.GetById(userId);
        }

        private async Task AssignRole(ApplicationUser applicationUser, string oldRole, string newRole)
        {
            if (!string.IsNullOrEmpty(oldRole))
            {
                await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
            }

            await _userManager.AddToRoleAsync(applicationUser, newRole);

            _userRepository.Update(applicationUser);
        }
    }
}
