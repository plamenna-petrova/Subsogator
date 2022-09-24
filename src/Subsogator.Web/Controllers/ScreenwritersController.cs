using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Screenwriters;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Common.GlobalConstants;

namespace Subsogator.Web.Controllers
{
    public class ScreenwritersController : BaseController
    {
        private readonly IScreenwriterService _screenwriterService;

        private readonly IUnitOfWork _unitOfWork;

        public ScreenwritersController(
            IScreenwriterService screenwriterService,
            IUnitOfWork unitOfWork
        )
        {
            _unitOfWork = unitOfWork;
            _screenwriterService = screenwriterService;
        }

        // GET: Screenwriters
        public IActionResult Index()
        {
            IEnumerable<AllScreenwritersViewModel> allScreenwritersViewModel =
                _screenwriterService
                    .GetAllScreenwriters();

            bool isAllScreenwritersViewModelEmpty = allScreenwritersViewModel.Count() == 0;

            if (isAllScreenwritersViewModelEmpty)
            {
                return NotFound();
            }

            return View(allScreenwritersViewModel);
        }

        // GET: Screenwriters/Details/5
        public IActionResult Details(string id)
        {
            ScreenwriterDetailsViewModel screenwriterDetailsViewModel = _screenwriterService
                .GetScreenwriterDetails(id);

            if (screenwriterDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(screenwriterDetailsViewModel);
        }

        // GET: Screenwriters/Create
        public ViewResult Create()
        {
            return View(new CreateScreenwriterBindingModel());
        }

        // POST: Screenwriters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateScreenwriterBindingModel createScreenwriterBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createScreenwriterBindingModel);
            }

            bool isNewScreenwriterCreated = _screenwriterService
                .CreateScreenwriter(createScreenwriterBindingModel);

            if (!isNewScreenwriterCreated)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                       NotificationMessages.ExistingRecordErrorMessage,
                       "screenwriter", $"{createScreenwriterBindingModel.FirstName} " +
                       $"{createScreenwriterBindingModel.LastName}"
                   );

                return View(createScreenwriterBindingModel);
            }

            bool isNewScreenwriterSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewScreenwriterSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage, "screenwriter"
                );

                return View(createScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                    NotificationMessages.RecordCreationSuccessMessage,
                    "Screenwriter", $"{createScreenwriterBindingModel.FirstName} " +
                    $"{createScreenwriterBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Screenwriters/Edit/5
        public IActionResult Edit(string id)
        {
            EditScreenwriterBindingModel editScreenwriterBindingModel = _screenwriterService
                .GetScreenwriterEditingDetails(id);

            if (editScreenwriterBindingModel == null)
            {
                return NotFound();
            }

            return View(editScreenwriterBindingModel);
        }

        // POST: Screenwriters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditScreenwriterBindingModel editScreenwriterBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editScreenwriterBindingModel);
            }

            bool isCurrentScreenwriterEdited = _screenwriterService
                .EditScreenwriter(editScreenwriterBindingModel);

            if (!isCurrentScreenwriterEdited)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.ExistingRecordErrorMessage,
                        "screenwriter", $"{editScreenwriterBindingModel.FirstName} " +
                        $"{editScreenwriterBindingModel.LastName}"
                    );

                return View(editScreenwriterBindingModel);
            }

            bool isCurrentScreenwriterUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentScreenwriterUpdateSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                    "screenwriter"
                  );

                return View(editScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                   NotificationMessages.RecordUpdateSuccessMessage,
                   "screenwriter", $"{editScreenwriterBindingModel.FirstName} " +
                   $"{editScreenwriterBindingModel.LastName}"
                );

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Screenwriters/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteScreenwriterViewModel deleteScreenwriterViewModel = _screenwriterService
                .GetScreenwriterDeletionDetails(id);

            if (deleteScreenwriterViewModel == null)
            {
                return NotFound();
            }

            return View(deleteScreenwriterViewModel);
        }

        // POST: Screenwriters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Screenwriter screenwriterToConfirmDeletion = _screenwriterService
                .FindScreenwriter(id);

            _screenwriterService.DeleteScreenwriter(screenwriterToConfirmDeletion);

            bool isScreenwriterDeleted = _unitOfWork.CommitSaveChanges();

            if (!isScreenwriterDeleted)
            {
                TempData["ScreenwriterErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedDeletionErrorMessage,
                    "screenwriter"
                   ) + $"{screenwriterToConfirmDeletion.FirstName} " +
                   $"{screenwriterToConfirmDeletion.LastName}";

                return RedirectToAction(nameof(Delete));
            }

            TempData["ScreenwriterSuccessMessage"] = string.Format(
                    NotificationMessages.RecordDeletionSuccessMessage,
                    "screenwriter", $"{screenwriterToConfirmDeletion.FirstName} " +
                    $"{screenwriterToConfirmDeletion.LastName}"
                  );

            return RedirectToIndexActionInCurrentController();
        }
    }
}
