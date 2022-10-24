using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Genres;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Genres.ViewModels;
using Subsogator.Web.Models.Genres.BindingModels;
using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Subsogator.Web.Controllers
{
    public class GenresController : BaseController
    {
        private readonly IGenreService _genreService;

        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IGenreService genreService, IUnitOfWork unitOfWork)
        {
            _genreService = genreService;
            _unitOfWork = unitOfWork;
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Index(
            string sortOrder,
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber)
        {
            IEnumerable<AllGenresViewModel> allGenresViewModel = _genreService
                .GetAllGenres();

            bool isAllGenresViewModelEmpty = allGenresViewModel.Count() == 0;

            if (isAllGenresViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["GenreNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "genre_name_descending"
                : "";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["GenreSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allGenresViewModel = allGenresViewModel
                        .Where(acvm =>
                            acvm.Name.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allGenresViewModel = sortOrder switch
            {
                "genre_name_descending" => allGenresViewModel
                        .OrderByDescending(acvm => acvm.Name),
                _ => allGenresViewModel.OrderBy(acvm => acvm.Name)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var paginatedList = PaginatedList<AllGenresViewModel>
                .Create(allGenresViewModel, pageNumber ?? 1, (int)pageSize);

            return View(paginatedList);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Details(string id)
        {
            GenreDetailsViewModel genreDetailsViewModel = _genreService.GetGenreDetails(id);

            if (genreDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(genreDetailsViewModel);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public ViewResult Create()
        {
            return View(new CreateGenreBindingModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateGenreBindingModel createGenreBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createGenreBindingModel);
            }

            bool isNewGenreCreated = _genreService.CreateGenre(createGenreBindingModel);

            if (!isNewGenreCreated)
            {
                TempData["GenreErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "genre",
                        createGenreBindingModel.Name);

                return View(createGenreBindingModel);
            }

            bool isNewGenreSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewGenreSavedToDatabase)
            {
                TempData["GenreErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage,
                    "genre");

                return View(createGenreBindingModel);
            }

            TempData["GenreSuccessMessage"] = string.Format(
                NotificationMessages.RecordCreationSuccessMessage,
                "Genre", $"{createGenreBindingModel.Name}");

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Edit(string id)
        {
            EditGenreBindingModel editGenreBindingModel = _genreService
                .GetGenreEditingDetails(id);

            if (editGenreBindingModel == null)
            {
                return NotFound();
            }

            return View(editGenreBindingModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditGenreBindingModel editGenreBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editGenreBindingModel);
            }

            bool isCurrentGenreEdited = _genreService.EditGenre(editGenreBindingModel);

            if (!isCurrentGenreEdited)
            {
                TempData["GenreErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "genre",
                        editGenreBindingModel.Name);

                return View(editGenreBindingModel);
            }

            bool isCurrentGenreUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentGenreUpdateSavedToDatabase)
            {
                TempData["GenreErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                        "genre");

                return View(editGenreBindingModel);
            }

            TempData["GenreSuccessMessage"] = string.Format(NotificationMessages
                .RecordUpdateSuccessMessage, "Genre",
                $"{editGenreBindingModel.Name}");

            return RedirectToIndexActionInCurrentController();
        }

        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        public IActionResult Delete(string id)
        {
            DeleteGenreViewModel deleteGenreViewModel = _genreService
                .GetGenreDeletionDetails(id);

            if (deleteGenreViewModel == null)
            {
                return NotFound();
            }

            return View(deleteGenreViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Genre genreToConfirmDeletion = _genreService.FindGenre(id);

            _genreService.DeleteGenre(genreToConfirmDeletion);

            bool isGenreDeleted = _unitOfWork.CommitSaveChanges();

            if (!isGenreDeleted)
            {
                TempData["GenreErrorMessage"] =
                    string.Format(NotificationMessages
                    .RecordFailedDeletionErrorMessage, "genre") +
                    $"{genreToConfirmDeletion.Name}";

                return RedirectToAction(nameof(Delete));
            }

            TempData["GenreSuccessMessage"] = string.Format(
                NotificationMessages.RecordDeletionSuccessMessage,
                "Genre", $"{genreToConfirmDeletion.Name}");

            return RedirectToIndexActionInCurrentController();
        }
    }
}
