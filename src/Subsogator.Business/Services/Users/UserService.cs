using Data.DataAccess.Repositories.Interfaces;
using Subsogator.Web.Models.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.Users
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<AllUsersViewModel> GetAllUsers()
        {
            var allUsers = _userRepository
                  .GetAllAsNoTracking()
                    .Select(u => new AllUsersViewModel
                    {
                        Username = u.UserName,
                        Roles = u.ApplicationUserRoles.Select(aur => aur.Role.Name).ToList()
                    });

            return allUsers;
        }
    }
}
