using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Languages;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Languages.ViewModels;
using Subsogator.Web.Models.Languages.BindingModels;
using Microsoft.Extensions.Logging;
using Subsogator.Common.GlobalConstants;

namespace Subsogator.Web.Controllers
{
    public class LanguagesController : BaseController
    {
        private readonly ILanguageService _languageService;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger _logger;

        public LanguagesController(
            ILanguageService languageService, 
            IUnitOfWork unitOfWork, 
            ILogger<LanguagesController> logger
        )
        {
            _languageService = languageService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: Languages
        public IActionResult Index()
        {
            IEnumerable<AllLanguagesViewModel> allLanguagesViewModel = _languageService
                .GetAllLanguagesWithRelatedData();

            bool isAllLanguagesViewModelEmpty = allLanguagesViewModel.Count() == 0;

            if (isAllLanguagesViewModelEmpty)
            {
                return NotFound();
            }

            return View(allLanguagesViewModel);
        }

        // GET: Languages/Details/5
        public IActionResult Details(string id)
        {
            LanguageDetailsViewModel languageDetailsViewModel = _languageService
                .GetLanguageDetails(id);

            if (languageDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(languageDetailsViewModel);
        }

        // GET: Languages/Create
        public ViewResult Create()
        {
            return View(new CreateLanguageBindingModel());
        }

        // POST: Languages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateLanguageBindingModel createLanguageBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createLanguageBindingModel);
            }

            bool isNewLanguageCreated = _languageService
                .CreateLanguage(createLanguageBindingModel);

            if (!isNewLanguageCreated)
            {
                TempData["LanguageErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "language",
                        createLanguageBindingModel.Name);

                return View(createLanguageBindingModel);
            }

            bool isNewLanguageSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewLanguageSavedToDatabase)
            {
                TempData["LanguageErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage,
                    "language");

                return View(createLanguageBindingModel);
            }

            TempData["LanguageSuccessMessage"] = string.Format(
                NotificationMessages.RecordCreationSuccessMessage,
                "Language", $"{createLanguageBindingModel.Name}");

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Languages/Edit/5
        public IActionResult Edit(string id)
        {
            EditLanguageBindingModel editLanguageBindingModel = _languageService
                .GetLanguageEditingDetails(id);

            if (editLanguageBindingModel == null)
            {
                return NotFound();
            }

            return View(editLanguageBindingModel);
        }

        // POST: Languages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditLanguageBindingModel editLanguageBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editLanguageBindingModel);
            }

            bool isCurrentLanguageEdited = _languageService.EditLanguage(editLanguageBindingModel);

            if (!isCurrentLanguageEdited)
            {
                TempData["LanguageErrorMessage"] = string.Format(NotificationMessages
                    .ExistingRecordErrorMessage, "language",
                        editLanguageBindingModel.Name);

                return View(editLanguageBindingModel);
            }

            bool isCurrentLanguageUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentLanguageUpdateSavedToDatabase)
            {
                TempData["LanguageErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                        "language");

                return View(editLanguageBindingModel);
            }

            TempData["LanguageSuccessMessage"] = string.Format(NotificationMessages
                .RecordUpdateSuccessMessage, "Language",
                $"{editLanguageBindingModel.Name}");

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Languages/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteLanguageViewModel deleteLanguageViewModel = _languageService
                .GetLanguageDeletionDetails(id);

            if (deleteLanguageViewModel == null)
            {
                return NotFound();
            }

            return View(deleteLanguageViewModel);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Language languageToConfirmDeletion = _languageService.FindLanguage(id);

            _languageService.DeleteLanguage(languageToConfirmDeletion);

            bool isLanguageDeleted = _unitOfWork.CommitSaveChanges();

            if (!isLanguageDeleted)
            {
                string failedDeletionMessage = NotificationMessages.RecordFailedDeletionErrorMessage;

                TempData["LanguageErrorMessage"] = 
                    string.Format(failedDeletionMessage, "language") + $"{languageToConfirmDeletion.Name}!"
                    + "Check the language relationship status!";

                return RedirectToAction(nameof(Delete));
            }

            TempData["LanguageSuccessMessage"] = string.Format(
                NotificationMessages.RecordDeletionSuccessMessage,
                "Language", $"{languageToConfirmDeletion.Name}");

            return RedirectToIndexActionInCurrentController();
        }
    }
}
