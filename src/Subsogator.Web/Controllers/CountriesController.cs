using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Business.Services.Countries;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Countries.BindingModels;

namespace Subsogator.Web.Controllers
{
    public class CountriesController : BaseController
    {
        private readonly ICountryService _countryService;

        private readonly IUnitOfWork _unitOfWork;

        public CountriesController(ICountryService countryService, IUnitOfWork unitOfWork)
        {
            _countryService = countryService;
            _unitOfWork = unitOfWork;
        }

        // GET: Countries
        public ViewResult Index()
        {
            IEnumerable<AllCountriesViewModel> allCountriesViewModel = _countryService.GetAllCountries();

            return View(allCountriesViewModel);
        }

        // GET: Countries/Details/5
        public IActionResult Details(string id)
        {
            CountryDetailsViewModel countryDetailsViewModel = _countryService.GetCountryDetails(id);

            if (countryDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(countryDetailsViewModel);
        }

        // GET: Countries/Create
        public ViewResult Create()
        {
            return View(new CreateCountryBindingModel());
        }

        // POST: Countries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCountryBindingModel createCountryBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createCountryBindingModel);
            }

            bool isNewCountryCreated = _countryService.CreateCountry(createCountryBindingModel);

            if (!isNewCountryCreated)
            {
                TempData["CountryErrorMessage"] = $"Error, the country " +
                    $"{createCountryBindingModel.Name} already exists!";
            }

            bool isNewCountrySavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewCountrySavedToDatabase)
            {
                TempData["CountryErrorMessage"] = "Error, couldn't save the new " +
                    "country record!";
                return View(createCountryBindingModel);
            }

            TempData["CountrySuccessMessage"] = $"Country {createCountryBindingModel.Name} " +
                $"created successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(string id)
        {
            EditCountryBindingModel editCountryBindingModel = _countryService
                .GetCountryEditingDetails(id);

            if (editCountryBindingModel == null)
            {
                return NotFound();
            }

            return View(editCountryBindingModel);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCountryBindingModel editCountryBindingModel)
        {
            if (!ModelState.IsValid) 
            {
                return View(editCountryBindingModel);
            }

            bool isCurrentCountryEdited = _countryService.EditCountry(editCountryBindingModel);

            if (!isCurrentCountryEdited)
            {
                TempData["CountryErrorMessage"] = $"Error, the country " +
                    $"{editCountryBindingModel.Name} already exists";
                return View(editCountryBindingModel);
            }

            bool isCurrentCountryUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentCountryUpdateSavedToDatabase)
            {
                TempData["CountryErrorMessage"] = $"Error, couldn't save " +
                    $"the current country update!";
                return View(editCountryBindingModel);
            }

            TempData["CountrySuccessMessage"] = $"Country {editCountryBindingModel.Name} " +
                $"saved successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Countries/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteCountryViewModel deleteCountryViewModel = _countryService
                .GetCountryDeletionDetails(id);

            if (deleteCountryViewModel == null)
            {
                return NotFound();
            }

            return View(deleteCountryViewModel);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Country countryToConfirmDeletion = _countryService.FindCountry(id);

            _countryService.DeleteCountry(countryToConfirmDeletion);

            bool isCountryDeleted = _unitOfWork.CommitSaveChanges();

            if (!isCountryDeleted)
            {
                TempData["CountryErrorMessage"] = $"Error, couldn't delete the country " +
                    $"{countryToConfirmDeletion.Name}!";
                return RedirectToAction(nameof(Delete));
            }

            TempData["CountrySuccessMessage"] = $"Country {countryToConfirmDeletion.Name} " +
                $"deleted successfully!";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
