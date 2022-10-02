using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Business.Services.FilmProductions;
using Subsogator.Business.Services.Countries;
using Subsogator.Business.Services.Languages;
using Subsogator.Web.Models.FilmProductions.BindingModels;
using Subsogator.Business.Transactions.Interfaces;
using Microsoft.Extensions.Logging;
using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Helpers;

namespace Subsogator.Web.Controllers
{
    public class FilmProductionsController : BaseController
    {
        private readonly IFilmProductionService _filmProductionService;

        private readonly ICountryService _countryService;

        private readonly ILanguageService _languageService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public FilmProductionsController(
            IFilmProductionService filmProductionService,
            ICountryService countryService,
            ILanguageService languageService,
            IUnitOfWork unitOfWork,
            ILogger<FilmProductionsController> logger
        )
        {
            _filmProductionService = filmProductionService;
            _countryService = countryService;
            _languageService = languageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: FilmProductions
        public IActionResult Index(
            string sortOrder,
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber)
        {
            IEnumerable<AllFilmProductionsViewModel> allFilmProductionsViewModel =
                _filmProductionService
                    .GetAllFilmProductionsWithRelatedData();

            bool isAllFilmProductionsViewModelEmpty =
                allFilmProductionsViewModel.Count() == 0;

            if (isAllFilmProductionsViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["FilmProductionTitleSort"] = string.IsNullOrEmpty(sortOrder)
                ? "film_production_title_descending"
                : "";
            ViewData["FilmProductionDurationSort"] = sortOrder == "film_production_duration_ascending"
                ? "film_production_duration_descending"
                : "film_production_duration_ascending";
            ViewData["FilmProductionReleaseDateSort"] = sortOrder == "film_production_release_date_ascending"
                ? "film_production_release_date_descending"
                : "film_production_release_date_ascending";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["FilmProductionSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allFilmProductionsViewModel = allFilmProductionsViewModel
                        .Where(afpvm =>
                            afpvm.Title.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allFilmProductionsViewModel = sortOrder switch
            {
                "film_production_title_descending" => allFilmProductionsViewModel
                        .OrderByDescending(afpvm => afpvm.Title),
                "film_production_duration_ascending" => allFilmProductionsViewModel
                        .OrderBy(afpvm => afpvm.Duration),
                "film_production_duration_descending" => allFilmProductionsViewModel
                        .OrderByDescending(afpvm => afpvm.Duration),
                "film_production_release_date_ascending" => allFilmProductionsViewModel
                        .OrderBy(afpvm => afpvm.ReleaseDate),
                "film_production_release_date_descending" => allFilmProductionsViewModel
                        .OrderByDescending(afpvm => afpvm.ReleaseDate),
                _ => allFilmProductionsViewModel.OrderBy(afpvm => afpvm.Title)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var paginatedList = PaginatedList<AllFilmProductionsViewModel>
                .Create(allFilmProductionsViewModel, pageNumber ?? 1, (int)pageSize);

            return View(paginatedList);
        }

        // GET: FilmProductions/Details/5
        public IActionResult Details(string id)
        {
            FilmProductionFullDetailsViewModel filmProductionFullDetailsViewModel =
                _filmProductionService
                    .GetFilmProductionDetails(id);

            if (filmProductionFullDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(filmProductionFullDetailsViewModel);
        }

        // GET: FilmProductions/Create
        public IActionResult Create()
        {
            var allCountriesForSelectList = _countryService.GetAllCountries();
            var allLanguagesForSelectList = _languageService.GetAllLanguages();

            ViewData["CountryByName"] = new SelectList(
                allCountriesForSelectList, "Id", "Name"
            );
            ViewData["LanguageByName"] = new SelectList(
                allLanguagesForSelectList, "Id", "Name"
            );

            return View(_filmProductionService.GetFilmProductionCreatingDetails());
        }

        // POST: FilmProductions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateFilmProductionBindingModel
            createFilmProductionBindingModel,
            string[] selectedGenres,
            string[] selectedActors,
            string[] selectedDirectors,
            string[] selectedScreenwriters
        )
        {
            if (!ModelState.IsValid)
            {
                return View(createFilmProductionBindingModel);
            }

            _filmProductionService.CreateFilmProduction(
                createFilmProductionBindingModel,
                selectedGenres,
                selectedActors,
                selectedDirectors,
                selectedScreenwriters
            );

            bool isNewFilmProductionSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewFilmProductionSavedToDatabase)
            {
                var allCountriesForSelectList = _countryService.GetAllCountries();
                var allLanguagesForSelectList = _languageService.GetAllLanguages();

                ViewData["CountryByName"] = new SelectList(
                            allCountriesForSelectList, "Id", "Name",
                            createFilmProductionBindingModel.CountryId
                        );
                ViewData["LanguageByName"] = new SelectList(
                            allLanguagesForSelectList, "Id", "Name",
                            createFilmProductionBindingModel.LanguageId
                        );

                TempData["FilmProductionErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage,
                    "film production");

                return View(createFilmProductionBindingModel);
            }

            TempData["FilmProductionSuccessMessage"] = string.Format(
                NotificationMessages.RecordCreationSuccessMessage,
                "Film Production", $"{createFilmProductionBindingModel.Title}");

            return RedirectToIndexActionInCurrentController();
        }

        // GET: FilmProductions/Edit/5
        public IActionResult Edit(string id)
        {
            EditFilmProductionBindingModel editFilmProductionBindingModel =
                _filmProductionService
                    .GetFilmProductionEditingDetails(id);

            if (editFilmProductionBindingModel == null)
            {
                return NotFound();
            }

            var allCountriesForSelectList = _countryService.GetAllCountries();
            var allLanguagesForSelectList = _languageService.GetAllLanguages();

            ViewData["CountryByName"] = new SelectList(
                    allCountriesForSelectList, "Id", "Name",
                    editFilmProductionBindingModel.CountryId
                );
            ViewData["LanguageByName"] = new SelectList(
                    allLanguagesForSelectList, "Id", "Name",
                    editFilmProductionBindingModel.LanguageId
                );

            return View(editFilmProductionBindingModel);
        }

        // POST: FilmProductions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(
            EditFilmProductionBindingModel editFilmProductionBindingModel,
            string[] selectedGenres,
            string[] selectedActors,
            string[] selectedDirectors,
            string[] selectedScreenwriters
        )
        {
            var allCountriesForSelectList = _countryService.GetAllCountries();
            var allLanguagesForSelectList = _languageService.GetAllLanguages();

            if (!ModelState.IsValid)
            {
                ViewData["CountryByName"] = new SelectList(
                        allCountriesForSelectList, "Id", "Name",
                        editFilmProductionBindingModel.CountryId
                    );
                ViewData["LanguageByName"] = new SelectList(
                        allLanguagesForSelectList, "Id", "Name",
                        editFilmProductionBindingModel.LanguageId
                    );

                return View(editFilmProductionBindingModel);
            }

            _filmProductionService.EditFilmProduction(
                editFilmProductionBindingModel,
                selectedGenres,
                selectedActors,
                selectedDirectors,
                selectedScreenwriters
            );

            bool isCurrentFilmProductionSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentFilmProductionSavedToDatabase)
            {
                ViewData["CountryByName"] = new SelectList(
                        allCountriesForSelectList, "Id", "Name",
                        editFilmProductionBindingModel.CountryId
                    );
                ViewData["LanguageByName"] = new SelectList(
                        allLanguagesForSelectList, "Id", "Name",
                        editFilmProductionBindingModel.LanguageId
                    );

                TempData["FilmProductionErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                        "film production");

                return View(editFilmProductionBindingModel);
            }

            TempData["FilmProductionSuccessMessage"] = string.Format(NotificationMessages
                .RecordUpdateSuccessMessage, "Film Production",
                $"{editFilmProductionBindingModel.Title}");

            return RedirectToIndexActionInCurrentController();
        }

        // GET: FilmProductions/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteFilmProductionViewModel deleteFilmProductionViewModel =
                _filmProductionService
                    .GetFilmProductionDeletionDetails(id);

            if (deleteFilmProductionViewModel == null)
            {
                return NotFound();
            }

            return View(deleteFilmProductionViewModel);
        }

        // POST: FilmProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            var filmProductionToConfirmDeletion = _filmProductionService
                .FindFilmProduction(id);

            _filmProductionService.DeleteFilmProduction(filmProductionToConfirmDeletion);

            bool isFilmProductionDeleted = _unitOfWork.CommitSaveChanges();

            if (!isFilmProductionDeleted)
            {
                string failedDeletionMessage = NotificationMessages
                    .RecordFailedDeletionErrorMessage;

                TempData["FilmProductionErrorMessage"] =
                    string.Format(failedDeletionMessage, "film production") +
                    $"{filmProductionToConfirmDeletion.Title}!"
                    + "Check the film production relationship status!";

                return RedirectToAction(nameof(Delete));
            }

            TempData["FilmProductionSuccessMessage"] = string.Format(
                NotificationMessages.RecordDeletionSuccessMessage,
                "Film Production", $"{filmProductionToConfirmDeletion.Title}");

            return RedirectToAction(nameof(Index));
        }
    }
}
