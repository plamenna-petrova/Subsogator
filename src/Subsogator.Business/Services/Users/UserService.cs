using Data.DataAccess;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities.Identity;
using Data.DataModels.Enums;
using Microsoft.AspNetCore.Identity;
using Subsogator.Web.Models.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using static Subsogator.Common.GlobalConstants.IdentityConstants;

namespace Subsogator.Business.Services.Users
{
    public class UserService: IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        private readonly IUserRepository _userRepository;

        private readonly ISubtitlesRepository _subtitlesRepository;

        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            ApplicationDbContext applicationDbContext,
            IUserRepository userRepository, 
            ISubtitlesRepository subtitlesRepository, 
            UserManager<ApplicationUser> userManager
        )
        {
            _applicationDbContext = applicationDbContext;
            _userRepository = userRepository;
            _subtitlesRepository = subtitlesRepository;
            _userManager = userManager;
        }

        public IEnumerable<AllUsersViewModel> GetAllUsers()
        {
            var allUsers = _userRepository
                  .GetAllAsNoTracking()
                    .Select(au => new AllUsersViewModel
                    {
                        Id = au.Id,
                        Username = au.UserName,
                        PromotionStatus = au.PromotionStatus,
                        PromotionLevel = au.PromotionLevel,
                        Roles = au.ApplicationUserRoles.Select(aur => aur.Role.Name).ToList()
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
                        case UploaderRoleName:
                            if (userRole == NormalUserRole)
                            {
                                await AssignRole(user, NormalUserRole, UploaderRoleName);
                                user.PromotionStatus = PromotionStatus.Accepted;
                                user.PromotionLevel = UploaderRoleName;
                            }
                            break;
                        case EditorRoleName:
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

        public async Task DemoteUser(string userId)
        {
            var user = FindUser(userId);
            var roles = _userManager.GetRolesAsync(user).Result.ToArray();

            if (roles.Length == 1) 
            {
                var userRole = roles[0];

                switch (userRole)
                {
                    case EditorRoleName:
                        await AssignRole(user, EditorRoleName, UploaderRoleName);
                        DeclinePromotion(user.Id);
                        break;
                    case UploaderRoleName:
                        await AssignRole(user, UploaderRoleName, NormalUserRole);
                        DeclinePromotion(user.Id);
                        break;
                }
            }
        }

        public void DeclinePromotion(string userId)
        {
            var user = FindUser(userId);

            if (user.PromotionStatus == PromotionStatus.Pending)
            {
                user.PromotionStatus = PromotionStatus.Declined;

                if (user.PromotionLevel != UploaderRoleName)
                {
                    user.PromotionLevel = null;
                }
            }

            _userRepository.Update(user);
        }

        public void EnrollForUploaderRole(string userId)
        {
            var user = FindUser(userId);

            user.PromotionStatus = PromotionStatus.Pending;
            user.PromotionLevel = UploaderRoleName;

            _userRepository.Update(user);
        }

        public void EnrollForEditorRole(string userId)
        {
            var user = FindUser(userId);

            user.PromotionStatus = PromotionStatus.Pending;
            user.PromotionLevel = EditorRoleName;

            _userRepository.Update(user);
        }

        public DeleteUserViewModel GetUserDeletionDetails(string userId)
        {
            var userToDelete = FindUser(userId);

            if (userToDelete is null)
            {
                return null;
            }

            var userToDeleteDetails = new DeleteUserViewModel
            {
                Id = userToDelete.Id,
                UserName = userToDelete.UserName,
                Role = userToDelete.ApplicationUserRoles.Select(aur => aur.Role).FirstOrDefault().Name
            };

            return userToDeleteDetails;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            try
            {
                var managedUserToDelete = await _userManager.FindByIdAsync(userId);
                var userLogins = managedUserToDelete.Logins;
                var userRoles = await _userManager.GetRolesAsync(managedUserToDelete);

                foreach (var userLogin in userLogins.ToList())
                {
                    await _userManager.RemoveLoginAsync(managedUserToDelete, userLogin.LoginProvider, userLogin.ProviderKey);
                }

                if (userRoles.Count() > 0)
                {
                    foreach (var userRole in userRoles.ToList())
                    {
                        await _userManager.RemoveFromRoleAsync(managedUserToDelete, userRole);
                    }
                }

                var subtitlesOfUser = _subtitlesRepository.GetAllAsNoTracking()
                    .Where(s => s.ApplicationUserId == managedUserToDelete.Id)
                    .ToArray();

                _subtitlesRepository.DeleteRange(subtitlesOfUser);

                await _userManager.DeleteAsync(managedUserToDelete);

                return true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return false;
            }
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
