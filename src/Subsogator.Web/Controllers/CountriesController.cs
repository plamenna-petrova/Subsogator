using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Data.DataModels.Entities;
using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Business.Services.Countries;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Countries.BindingModels;
using Microsoft.Extensions.Logging;
using Subsogator.Common.GlobalConstants;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Subsogator.Common.Helpers;

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

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Index(
            string sortOrder,
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber)
        {
            IEnumerable<AllCountriesViewModel> allCountriesViewModel = _countryService
                .GetAllCountriesWithRelatedData();

            bool isAllCountriesViewModelEmpty = allCountriesViewModel.Count() == 0;

            if (isAllCountriesViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["CountryNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "country_name_descending"
                : "";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["CountrySearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allCountriesViewModel = allCountriesViewModel
                        .Where(acvm =>
                            acvm.Name.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allCountriesViewModel = sortOrder switch
            {
                "country_name_descending" => allCountriesViewModel
                        .OrderByDescending(acvm => acvm.Name),
                _ => allCountriesViewModel.OrderBy(acvm => acvm.Name)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var countriesPaginatedList = PaginatedList<AllCountriesViewModel>
                .Create(allCountriesViewModel, pageNumber ?? 1, (int)pageSize);

            return View(countriesPaginatedList);
        }

        [Authorize(Roles = "Administrator, Editor")]
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

        [Authorize(Roles = "Administrator, Editor")]
        public ViewResult Create()
        {
            return View(new CreateCountryBindingModel());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateCountryBindingModel createCountryBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createCountryBindingModel);
            }

            bool isNewCountryCreated = _countryService.CreateCountry(
                createCountryBindingModel, User.FindFirstValue(ClaimTypes.Name)
            );

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

        [Authorize(Roles = "Administrator, Editor")]
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

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCountryBindingModel editCountryBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editCountryBindingModel);
            }

            bool isCurrentCountryEdited = _countryService.EditCountry(
                editCountryBindingModel, User.FindFirstValue(ClaimTypes.Name)
            );

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

        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
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
