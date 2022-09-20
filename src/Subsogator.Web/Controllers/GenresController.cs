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
            IEnumerable<AllGenresViewModel> allGenresViewModel = _genreService.GetAllGenres();

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
                TempData["GenreErrorMessage"] = $"Error, the genre " +
                    $"{createGenreBindingModel.Name} already exists!";
            }

            bool isNewGenreSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewGenreSavedToDatabase)
            {
                TempData["GenreErrorMessage"] = "Error, couldn't save the new " +
                    "genre record!";
                return View(createGenreBindingModel);
            }

            TempData["GenreSuccessMessage"] = $"Genre {createGenreBindingModel.Name} " +
                $"created successfully!";

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
                TempData["GenreErrorMessage"] = $"Error, the genre " +
                    $"{editGenreBindingModel.Name} already exists";
                return View(editGenreBindingModel);
            }

            bool isCurrentGenreUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentGenreUpdateSavedToDatabase)
            {
                TempData["GenreErrorMessage"] = $"Error, couldn't save " +
                    $"the current genre update!";
                return View(editGenreBindingModel);
            }

            TempData["GenreSuccessMessage"] = $"Genre {editGenreBindingModel.Name} " +
                $"saved successfully!";

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
                TempData["GenreErrorMessage"] = $"Error, couldn't delete the genre " +
                    $"{genreToConfirmDeletion.Name}!";
                return RedirectToAction(nameof(Delete));
            }

            TempData["GenreSuccessMessage"] = $"Genre {genreToConfirmDeletion.Name} " +
                $"deleted successfully!";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
