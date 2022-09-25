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

        // GET: Genres
        public IActionResult Index()
        {
            IEnumerable<AllGenresViewModel> allGenresViewModel = 
                _genreService.GetAllGenres();

            bool isAllGenresViewModelEmpty = allGenresViewModel.Count() == 0;

            if (isAllGenresViewModelEmpty)
            {
                return NotFound();
            }

            return View(allGenresViewModel);
        }

        // GET: Genres/Details/5
        public IActionResult Details(string id)
        {
            GenreDetailsViewModel genreDetailsViewModel = _genreService.GetGenreDetails(id);

            if (genreDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(genreDetailsViewModel);
        }

        // GET: Genres/Create
        public ViewResult Create()
        {
            return View(new CreateGenreBindingModel());
        }

        // POST: Genres/Create
        [HttpPost]
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

        // GET: Genres/Edit/5
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

        // POST: Genres/Edit/5
        [HttpPost]
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

        // GET: Genres/Delete/5
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

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
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
