using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subsogator.Business.Services.Users;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Common.GlobalConstants;
using Subsogator.Common.Helpers;
using Subsogator.Web.Models.Users.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Subsogator.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUserService userService, IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(
                    string sortOrder,
                    string currentFilter,
                    string searchTerm,
                    int? pageSize,
                    int? pageNumber)
        {
            IEnumerable<AllUsersViewModel> allUsersViewModel = _userService.GetAllUsers();

            bool isAlUsersViewModelEmpty = allUsersViewModel.Count() == 0;

            if (isAlUsersViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "user_name_descending"
                : "";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["UserSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allUsersViewModel = allUsersViewModel
                        .Where(acvm =>
                            acvm.Username.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allUsersViewModel = sortOrder switch
            {
                "user_name_descending" => allUsersViewModel
                        .OrderByDescending(acvm => acvm.Username),
                _ => allUsersViewModel.OrderBy(acvm => acvm.Username)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var usersPaginatedList = PaginatedList<AllUsersViewModel>
                .Create(allUsersViewModel, pageNumber ?? 1, (int)pageSize);

            return View(usersPaginatedList);
        }

        public async Task<IActionResult> Promote(string id)
        {
            await _userService.PromoteUser(id);
            _unitOfWork.CommitSaveChanges();

            return RedirectToIndexActionInCurrentController();
        }

        public IActionResult DeclinePromotion(string id)
        {
            _userService.DeclinePromotion(id);
            _unitOfWork.CommitSaveChanges();

            return RedirectToIndexActionInCurrentController();
        }

        public async Task<IActionResult> Demote(string id)
        {
            await _userService.DemoteUser(id);
            _unitOfWork.CommitSaveChanges();

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(string id)
        {
            DeleteUserViewModel deleteUserViewModel = _userService.GetUserDeletionDetails(id);

            if (deleteUserViewModel == null)
            {
                return NotFound();
            }

            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == deleteUserViewModel.Id)
            {
                TempData["UsersInvalidOperationErrorMessage"] = "Error! The admin cannot delete himself!";

                return RedirectToIndexActionInCurrentController();
            }

            return View(deleteUserViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userToConfirmDeletion = _userService.FindUser(id);

            bool isUserDeleted = await _userService.DeleteUser(userToConfirmDeletion.Id);

            if (isUserDeleted)
            {
                _unitOfWork.CommitSaveChanges();
            }
            else
            {
                string userFailedDeletionErrorMessage = NotificationMessages
                    .RecordFailedDeletionErrorMessage;

                TempData["UsersErrorMessage"] =
                    string.Format(userFailedDeletionErrorMessage, "user") +
                    $" {userToConfirmDeletion.UserName}!";

                return RedirectToAction(nameof(Delete));
            }

            TempData["UsersSuccessMessage"] = string.Format(
                NotificationMessages.RecordDeletionSuccessMessage,
                "User", $"{userToConfirmDeletion.UserName}"
            );

            return RedirectToIndexActionInCurrentController();
        }
    } 
}
