using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Screenwriters;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Common.GlobalConstants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Subsogator.Common.Helpers;

namespace Subsogator.Web.Controllers
{
    public class ScreenwritersController : BaseController
    {
        private readonly IScreenwriterService _screenwriterService;

        private readonly IUnitOfWork _unitOfWork;

        public ScreenwritersController(
            IScreenwriterService screenwriterService, 
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
            _screenwriterService = screenwriterService;
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Index(
            string sortOrder,
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber
        )
        {
            IEnumerable<AllScreenwritersViewModel> allScreenwritersViewModel = _screenwriterService.GetAllScreenwriters();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ScreenwriterFirstNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "screenwriter_first_name_descending"
                : "";
            ViewData["ScreenwriterLastNameSort"] = sortOrder == "screenwriter_last_name_ascending"
                ? "screenwriter_last_name_descending"
                : "screenwriter_last_name_ascending";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["ScreenwriterSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allScreenwritersViewModel = allScreenwritersViewModel
                        .Where(asvm =>
                            asvm.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                            asvm.LastName.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allScreenwritersViewModel = sortOrder switch
            {
                "screenwriter_first_name_descending" => allScreenwritersViewModel
                        .OrderByDescending(asvm => asvm.FirstName),
                "screenwriter_last_name_ascending" => allScreenwritersViewModel
                        .OrderBy(asvm => asvm.LastName),
                "screenwriter_last_name_descending" => allScreenwritersViewModel
                        .OrderByDescending(asvm => asvm.LastName),
                _ => allScreenwritersViewModel.OrderBy(asvm => asvm.FirstName)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var screenwritersPaginatedList = PaginatedList<AllScreenwritersViewModel>
                .Create(allScreenwritersViewModel, pageNumber ?? 1, (int)pageSize);

            return View(screenwritersPaginatedList);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Details(string id)
        {
            ScreenwriterDetailsViewModel screenwriterDetailsViewModel = _screenwriterService
                .GetScreenwriterDetails(id);

            if (screenwriterDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(screenwriterDetailsViewModel);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public ViewResult Create()
        {

            return View(_screenwriterService.GetScreenwriterCreatingDetails());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            CreateScreenwriterBindingModel createScreenwriterBindingModel,
            string[] selectedFilmProductions
         )
        {
            if (!ModelState.IsValid)
            {
                return View(createScreenwriterBindingModel);
            }

            bool isNewScreenwriterCreated = _screenwriterService.CreateScreenwriter(
                createScreenwriterBindingModel, selectedFilmProductions, User.FindFirstValue(ClaimTypes.Name)
            );

            if (!isNewScreenwriterCreated)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "screenwriter", $"{createScreenwriterBindingModel.FirstName} " +
                        $"{createScreenwriterBindingModel.LastName}"
                    );

                return View(createScreenwriterBindingModel);
            }

            bool isNewScreenwriterSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewScreenwriterSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage, "screenwriter"
                );

                return View(createScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                    NotificationMessages.RecordCreationSuccessMessage,
                    "Screenwriter", $"{createScreenwriterBindingModel.FirstName} " +
                    $"{createScreenwriterBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Edit(string id)
        {
            EditScreenwriterBindingModel editScreenwriterBindingModel = _screenwriterService
                .GetScreenwriterEditingDetails(id);

            if (editScreenwriterBindingModel == null)
            {
                return NotFound();
            }

            return View(editScreenwriterBindingModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            EditScreenwriterBindingModel editScreenwriterBindingModel,
            string[] selectedFilmProductions
        )
        {
            if (!ModelState.IsValid)
            {
                return View(editScreenwriterBindingModel);
            }

            bool isCurrentScreenwriterEdited = _screenwriterService.EditScreenwriter(
                editScreenwriterBindingModel, selectedFilmProductions, User.FindFirstValue(ClaimTypes.Name)
            );

            if (!isCurrentScreenwriterEdited)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "screenwriter", $"{editScreenwriterBindingModel.FirstName} " +
                        $"{editScreenwriterBindingModel.LastName}"
                    );

                return View(editScreenwriterBindingModel);
            }

            bool isCurrentScreenwriterUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentScreenwriterUpdateSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                    "screenwriter"
                  );

                return View(editScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                   NotificationMessages.RecordUpdateSuccessMessage,
                   "Screenwriter", $"{editScreenwriterBindingModel.FirstName} " +
                   $"{editScreenwriterBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        public IActionResult Delete(string id)
        {
            DeleteScreenwriterViewModel deleteScreenwriterViewModel = _screenwriterService
                .GetScreenwriterDeletionDetails(id);

            if (deleteScreenwriterViewModel == null)
            {
                return NotFound();
            }

            return View(deleteScreenwriterViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Screenwriter screenwriterToConfirmDeletion = _screenwriterService.FindScreenwriter(id);

            _screenwriterService.DeleteScreenwriter(screenwriterToConfirmDeletion);

            bool isScreenwriterDeleted = _unitOfWork.CommitSaveChanges();

            if (!isScreenwriterDeleted)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedDeletionErrorMessage,
                    "screenwriter"
                   ) + $" {screenwriterToConfirmDeletion.FirstName} " +
                   $"{screenwriterToConfirmDeletion.LastName}";

                return RedirectToAction(nameof(Delete));
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                    NotificationMessages.RecordDeletionSuccessMessage,
                    "Screenwriter", $"{screenwriterToConfirmDeletion.FirstName} " +
                    $"{screenwriterToConfirmDeletion.LastName}"
                  );

            return RedirectToIndexActionInCurrentController();
        }
    }
}
