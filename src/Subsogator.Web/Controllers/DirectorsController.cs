using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.DataModels.Entities;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Business.Services.Directors;
using Subsogator.Web.Models.Directors.ViewModels;
using Subsogator.Web.Models.Directors.BindingModels;
using Subsogator.Common.GlobalConstants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Subsogator.Common.Helpers;

namespace Subsogator.Web.Controllers
{
    public class DirectorsController : BaseController
    {
        private readonly IDirectorService _directorService;

        private readonly IUnitOfWork _unitOfWork;

        public DirectorsController(IDirectorService directorService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _directorService = directorService;
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
            IEnumerable<AllDirectorsViewModel> allDirectorsViewModel = _directorService
                    .GetAllDirectors();

            bool isAllDirectorsViewModelEmpty = allDirectorsViewModel.Count() == 0;

            if (isAllDirectorsViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["DirectorFirstNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "director_first_name_descending"
                : "";
            ViewData["DirectorLastNameSort"] = sortOrder == "director_last_name_ascending"
                ? "director_last_name_descending"
                : "director_last_name_ascending";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["DirectorSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allDirectorsViewModel = allDirectorsViewModel
                        .Where(advm =>
                            advm.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                            advm.LastName.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allDirectorsViewModel = sortOrder switch
            {
                "director_first_name_descending" => allDirectorsViewModel
                        .OrderByDescending(advm => advm.FirstName),
                "director_last_name_ascending" => allDirectorsViewModel
                        .OrderBy(advm => advm.LastName),
                "director_last_name_descending" => allDirectorsViewModel
                        .OrderByDescending(advm => advm.LastName),
                _ => allDirectorsViewModel.OrderBy(advm => advm.FirstName)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var directorsPaginatedList = PaginatedList<AllDirectorsViewModel>
                .Create(allDirectorsViewModel, pageNumber ?? 1, (int)pageSize);

            return View(directorsPaginatedList);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Details(string id)
        {
            DirectorDetailsViewModel directorDetailsViewModel = _directorService
                .GetDirectorDetails(id);

            if (directorDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(directorDetailsViewModel);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public ViewResult Create()
        {
            return View(_directorService.GetDirectorCreatingDetails());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            CreateDirectorBindingModel createDirectorBindingModel,
            string[] selectedFilmProductions
         )
        {
            if (!ModelState.IsValid)
            {
                return View(createDirectorBindingModel);
            }

            bool isNewDirectorCreated = _directorService.CreateDirector(
                createDirectorBindingModel, selectedFilmProductions, User.FindFirstValue(ClaimTypes.Name)
            );

            if (!isNewDirectorCreated)
            {
                TempData["DirectorErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "director", $"{createDirectorBindingModel.FirstName} " +
                        $"{createDirectorBindingModel.LastName}"
                    );

                return View(createDirectorBindingModel);
            }

            bool isNewDirectorSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewDirectorSavedToDatabase)
            {
                TempData["DirectorErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage, "director"
                );

                return View(createDirectorBindingModel);
            }

            TempData["DirectorSuccessMessage"] = string.Format(
                    NotificationMessages.RecordCreationSuccessMessage,
                    "Director", $"{createDirectorBindingModel.FirstName} " +
                    $"{createDirectorBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Edit(string id)
        {
            EditDirectorBindingModel editDirectorBindingModel = _directorService
                .GetDirectorEditingDetails(id);

            if (editDirectorBindingModel == null)
            {
                return NotFound();
            }

            return View(editDirectorBindingModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            EditDirectorBindingModel editDirectorBindingModel,
            string[] selectedFilmProductions
        )
        {
            if (!ModelState.IsValid)
            {
                return View(editDirectorBindingModel);
            }

            bool isCurrentDirectorEdited = _directorService.EditDirector(
                editDirectorBindingModel, selectedFilmProductions, User.FindFirstValue(ClaimTypes.Name)
            );

            if (!isCurrentDirectorEdited)
            {
                TempData["DirectorErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "director", $"{editDirectorBindingModel.FirstName} " +
                        $"{editDirectorBindingModel.LastName}"
                    );

                return View(editDirectorBindingModel);
            }

            bool isCurrentDirectorUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentDirectorUpdateSavedToDatabase)
            {
                TempData["DirectorErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                    "director"
                  );

                return View(editDirectorBindingModel);
            }

            TempData["DirectorSuccessMessage"] = string.Format(
                   NotificationMessages.RecordUpdateSuccessMessage,
                   "Director", $"{editDirectorBindingModel.FirstName} " +
                   $"{editDirectorBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        public IActionResult Delete(string id)
        {
            DeleteDirectorViewModel deleteDirectorViewModel = _directorService
                .GetDirectorDeletionDetails(id);

            if (deleteDirectorViewModel == null)
            {
                return NotFound();
            }

            return View(deleteDirectorViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Director directorToConfirmDeletion = _directorService.FindDirector(id);

            _directorService.DeleteDirector(directorToConfirmDeletion);

            bool isDirectorDeleted = _unitOfWork.CommitSaveChanges();

            if (!isDirectorDeleted)
            {
                TempData["DirectorErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedDeletionErrorMessage,
                    "director"
                   ) + $"{directorToConfirmDeletion.FirstName} " +
                   $"{directorToConfirmDeletion.LastName}";

                return RedirectToAction(nameof(Delete));
            }

            TempData["DirectorSuccessMessage"] = string.Format(
                    NotificationMessages.RecordDeletionSuccessMessage,
                    "Director", $"{directorToConfirmDeletion.FirstName} " +
                    $"{directorToConfirmDeletion.LastName}"
                  );

            return RedirectToIndexActionInCurrentController();
        }
    }
}
