using Data.DataModels.Entities.Identity;
using Data.DataModels.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subsogator.Business.Services.Users;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Models;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Subsogator.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IUserService userService, 
            IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager,
            ILogger<HomeController> logger
        )
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(Index), "SubtitlesCatalogue");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> BecomeAnUploader()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.PromotionStatus == PromotionStatus.Pending)
            {
                TempData["UserPromotionInfoMessage"] = "The current status for uploader promotion is pending!";

                return RedirectToIndexActionInCurrentController();
            }

            if (currentUser.PromotionStatus == PromotionStatus.Declined)
            {
                _userService.EnrollForUploaderRole(currentUser.Id);
                _unitOfWork.CommitSaveChanges();

                TempData["UserPromotionInfoMessage"] = "The uploader promotion was declined. Sending another request!";

                return RedirectToIndexActionInCurrentController();
            }

            _userService.EnrollForUploaderRole(currentUser.Id);
            _unitOfWork.CommitSaveChanges();

            TempData["UserPromotionInfoMessage"] = "Request for uploader promotion sent. Status - pending";

            return RedirectToIndexActionInCurrentController();
        }

        public async Task<IActionResult> BecomeAnEditor()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser.PromotionStatus == PromotionStatus.Pending)
            {
                TempData["UserPromotionInfoMessage"] = "The current status for editor promotion is pending!";
            }

            if (currentUser.PromotionStatus == PromotionStatus.Declined)
            {
                _userService.EnrollForEditorRole(currentUser.Id);
                _unitOfWork.CommitSaveChanges();

                TempData["UserPromotionInfoMessage"] = "The editor promotion was declined. Sending another request!";

                return RedirectToIndexActionInCurrentController();
            }

            _userService.EnrollForEditorRole(currentUser.Id);
            _unitOfWork.CommitSaveChanges();

            TempData["UserPromotionInfoMessage"] = "Request for editor promotion sent. Status - pending";

            return RedirectToIndexActionInCurrentController();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
