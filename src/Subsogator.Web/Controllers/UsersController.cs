using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Subsogator.Business.Services.Users;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Helpers;
using Subsogator.Web.Models.Users.ViewModels;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                "User_name_descending" => allUsersViewModel
                        .OrderByDescending(acvm => acvm.Username),
                _ => allUsersViewModel.OrderBy(acvm => acvm.Username)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var paginatedList = PaginatedList<AllUsersViewModel>
                .Create(allUsersViewModel, pageNumber ?? 1, (int)pageSize);

            return View(paginatedList);
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
    }
}
