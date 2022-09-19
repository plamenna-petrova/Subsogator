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

namespace Subsogator.Web.Controllers
{
    public class FilmProductionsController : BaseController
    {
        private readonly IFilmProductionService _filmProductionService;

        private readonly ICountryService _countryService;

        private readonly ILanguageService _languageService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        private readonly ApplicationDbContext _context;

        public FilmProductionsController(
            IFilmProductionService filmProductionService,
            ICountryService countryService,
            ILanguageService languageService,
            IUnitOfWork unitOfWork,
            ILogger<FilmProductionsController> logger,
            ApplicationDbContext context
        )
        {
            _filmProductionService = filmProductionService;
            _countryService = countryService;
            _languageService = languageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _context = context;
        }

        // GET: FilmProductions
        public IActionResult Index()
        {
            IEnumerable<AllFilmProductionsViewModel> allFilmProductionsViewModel = _filmProductionService
                .GetAllFilmProductions();

            return View(allFilmProductionsViewModel);
        }

        // GET: FilmProductions/Details/5
        public IActionResult Details(string id)
        {
            FilmProductionFullDetailsViewModel filmProductionFullDetailsViewModel = _filmProductionService
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

            ViewData["CountryByName"] = new SelectList(allCountriesForSelectList, "Id", "Name");
            ViewData["LanguageByName"] = new SelectList(allLanguagesForSelectList, "Id", "Name");

            return View();
        }

        // POST: FilmProductions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateFilmProductionBindingModel createFilmProductionBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createFilmProductionBindingModel);
            }

            _filmProductionService.CreateFilmProduction(createFilmProductionBindingModel);

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

                TempData["FilmProductionErrorMessage"] = $"Error, couldn't save the new film production" +
                    $"{createFilmProductionBindingModel.Title}!";

                return View(createFilmProductionBindingModel);
            }

            TempData["FilmProductionSuccessMessage"] = $"Film Production " +
                    $"{createFilmProductionBindingModel.Title} " +
                $"created successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: FilmProductions/Edit/5
        public IActionResult Edit(string id)
        {
            EditFilmProductionBindingModel editFilmProductionBindingModel = _filmProductionService
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
        public IActionResult Edit(EditFilmProductionBindingModel editFilmProductionBindingModel)
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

            _filmProductionService.EditFilmProduction(editFilmProductionBindingModel);

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

                TempData["FilmProductionErrorMessage"] = $"Error, couldn't save " +
                    $"the current film production update!";

                return View(editFilmProductionBindingModel);
            }

            TempData["FilmProductionSuccessMessage"] = $"Film Production " +
                $"{editFilmProductionBindingModel.Title} saved successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: FilmProductions/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteFilmProductionViewModel deleteFilmProductionViewModel = _filmProductionService
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
            var filmProductionToConfirmDeletion = _filmProductionService.FindFilmProduction(id);

            _filmProductionService.DeleteFilmProduction(filmProductionToConfirmDeletion);

            bool isFilmProductionDeleted = _unitOfWork.CommitSaveChanges();

            if (!isFilmProductionDeleted)
            {
                TempData["FilmProductionErrorMessage"] = $"Error, couldn't delete the film production " +
                     $"{filmProductionToConfirmDeletion.Title}! Check the " +
                         $"film production relationship status!";

                return RedirectToAction(nameof(Delete));
            }

            TempData["FilmProductionSuccessMessage"] = $"Film Production {filmProductionToConfirmDeletion.Title} " +
                $"deleted successfully!";

            return RedirectToAction(nameof(Index));
        }
    }
}
