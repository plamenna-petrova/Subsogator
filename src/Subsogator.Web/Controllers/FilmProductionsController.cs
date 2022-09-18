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
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmProduction = await _context.FilmProductions
                .Include(f => f.Country)
                .Include(f => f.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmProduction == null)
            {
                return NotFound();
            }

            return View(filmProduction);
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

            try
            {
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

                    return View(createFilmProductionBindingModel);
                }

                TempData["CountrySuccessMessage"] = $"Film Production " +
                        $"{createFilmProductionBindingModel.Title} " +
                    $"created successfully!";

                return RedirectToIndexActionInCurrentController();
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError("Exception: " + dbUpdateException.Message + "\n" + "Inner Exception :" +
                    dbUpdateException.InnerException.Message ?? "");

                TempData["LanguageErrorMessage"] = $"Error, couldn't save the new film production " +
                    $"{createFilmProductionBindingModel.Title}! Check the " +
                    $"language relationship status!";

                return RedirectToAction(nameof(Create));
            }
        }

        // GET: FilmProductions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmProduction = await _context.FilmProductions.FindAsync(id);
            if (filmProduction == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", filmProduction.CountryId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Id", filmProduction.LanguageId);
            return View(filmProduction);
        }

        // POST: FilmProductions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Title,Duration,ReleaseDate,PlotSummary,CountryId,LanguageId,Id,CreatedOn,ModifiedOn")] FilmProduction filmProduction)
        {
            if (id != filmProduction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmProduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmProductionExists(filmProduction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", filmProduction.CountryId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Id", filmProduction.LanguageId);
            return View(filmProduction);
        }

        // GET: FilmProductions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmProduction = await _context.FilmProductions
                .Include(f => f.Country)
                .Include(f => f.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmProduction == null)
            {
                return NotFound();
            }

            return View(filmProduction);
        }

        // POST: FilmProductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var filmProduction = await _context.FilmProductions.FindAsync(id);
            _context.FilmProductions.Remove(filmProduction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmProductionExists(string id)
        {
            return _context.FilmProductions.Any(e => e.Id == id);
        }
    }
}
