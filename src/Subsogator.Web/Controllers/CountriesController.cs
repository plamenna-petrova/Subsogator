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
using Microsoft.Extensions.Logging;
using Subsogator.Common.GlobalConstants;

namespace Subsogator.Web.Controllers
{
    public class CountriesController : BaseController
    {
        private readonly ICountryService _countryService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public CountriesController(
            ICountryService countryService,
            IUnitOfWork unitOfWork,
            ILogger<CountriesController> logger
        )
        {
            _countryService = countryService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Countries
        public IActionResult Index()
        {
            IEnumerable<AllCountriesViewModel> allCountriesViewModel = _countryService
                .GetAllCountriesWithRelatedData();

            bool isAllCountriesViewModelEmpty = allCountriesViewModel.Count() == 0;

            if (isAllCountriesViewModelEmpty)
            {
                return NotFound();
            }

            return View(allCountriesViewModel);
        }

        // GET: Countries/Details/5
        public IActionResult Details(string id)
        {
            CountryDetailsViewModel countryDetailsViewModel = _countryService
                .GetCountryDetails(id);

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

            bool isNewCountryCreated = _countryService
                .CreateCountry(createCountryBindingModel);

            if (!isNewCountryCreated)
            {
                TempData["CountryErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "country",
                        createCountryBindingModel.Name);

                return View(createCountryBindingModel);
            }

            bool isNewCountrySavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewCountrySavedToDatabase)
            {
                TempData["CountryErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage, 
                    "country");

                return View(createCountryBindingModel);
            }

            TempData["CountrySuccessMessage"] = string.Format(
                NotificationMessages.RecordCreationSuccessMessage, 
                "Country", $"{createCountryBindingModel.Name}");

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCountryBindingModel editCountryBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCountryBindingModel);
            }

            bool isCurrentCountryEdited = _countryService
                .EditCountry(editCountryBindingModel);

            if (!isCurrentCountryEdited)
            {
                TempData["CountryErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "country",
                        editCountryBindingModel.Name);

                return View(editCountryBindingModel);
            }

            bool isCurrentCountryUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentCountryUpdateSavedToDatabase)
            {
                TempData["CountryErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage, 
                    "country");

                return View(editCountryBindingModel);
            }

            TempData["CountrySuccessMessage"] = string.Format(NotificationMessages
                .RecordUpdateSuccessMessage, "Country",
                $"{editCountryBindingModel.Name}");

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
                string failedDeletionMessage = NotificationMessages
                    .RecordFailedDeletionErrorMessage;

                TempData["CountryErrorMessage"] =
                    string.Format(failedDeletionMessage, "country") + 
                    $"{countryToConfirmDeletion.Name}!"
                    + "Check the country relationship status!";

                return RedirectToAction(nameof(Delete));
            }

            TempData["CountrySuccessMessage"] = string.Format(
                NotificationMessages.RecordDeletionSuccessMessage, 
                "Country", $"{countryToConfirmDeletion.Name}");

            return RedirectToIndexActionInCurrentController();

        }
    }
}
