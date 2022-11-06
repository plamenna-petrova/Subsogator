using Subsogator.Web.Models.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Users
{
    public interface IUserService
    {
        IEnumerable<AllUsersViewModel> GetAllUsers();
    }
}
