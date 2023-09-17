using Data.DataModels.Entities;
using Data.DataModels.Entities.Identity;
using Subsogator.Web.Models.Users.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subsogator.Business.Services.Users
{
    public interface IUserService
    {
        IEnumerable<AllUsersViewModel> GetAllUsers();

        Task PromoteUser(string userId);

        ApplicationUser FindUser(string userId);

        void DeclinePromotion(string userId);

        Task DemoteUser(string userId);

        void EnrollForUploaderRole(string userId);

        void EnrollForEditorRole(string userId);
    }
}
