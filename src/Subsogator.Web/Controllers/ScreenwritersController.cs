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

namespace Subsogator.Web.Controllers
{
    public class ScreenwritersController : BaseController
    {
        private readonly IScreenwriterService _screenwriterService;

        private readonly IUnitOfWork _unitOfWork;

        public ScreenwritersController(IScreenwriterService screenwriterService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _screenwriterService = screenwriterService;
        }

        // GET: Screenwriters
        public ViewResult Index()
        {
            IEnumerable<AllScreenwritersViewModel> allScreenwritersViewModel = _screenwriterService
                .GetAllScreenwriters();

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
                TempData["ScreenwriterErrorMessage"] = $"Error, the screenwriter" +
                    $"{createScreenwriterBindingModel.FirstName}" +
                    $"{createScreenwriterBindingModel.LastName} already exists";
                return View(createScreenwriterBindingModel);
            }

            bool isNewScreenwriterSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewScreenwriterSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = "Error, couldn't save the new" +
                    "screenwriter record";
                return View(createScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = $"Screenwriter {createScreenwriterBindingModel.FirstName} " +
                $"{createScreenwriterBindingModel.LastName} created successfully!";

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
                TempData["ScreenwriterErrorMessage"] = $"Error, the screenwriter " +
                $"{editScreenwriterBindingModel.FirstName} " +
                    $"{editScreenwriterBindingModel.LastName} already exists";
                return View(editScreenwriterBindingModel);
            }

            bool isCurrentScreenwriterUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentScreenwriterUpdateSavedToDatabase)
            {
                TempData["ScreenwriterErrorMessage"] = "Error, couldn't save the current" +
                    "screenwriter update";
                return View(editScreenwriterBindingModel);
            }

            TempData["ScreenwriterSuccessMessage"] = $"Screenwriter {editScreenwriterBindingModel.FirstName} " +
                $"{editScreenwriterBindingModel.LastName} saved successfully!";

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
            Screenwriter screenwriterToConfirmDeletion = _screenwriterService.FindScreenwriter(id);

            _screenwriterService.DeleteScreenwriter(screenwriterToConfirmDeletion);

            bool isScreenwriterDeleted = _unitOfWork.CommitSaveChanges();

            if (!isScreenwriterDeleted)
            {
                TempData["ScreenwriterErrorMessage"] = "Error, couldn't delete the screenwriter!";
                return RedirectToAction(nameof(Delete));
            }

            TempData["ScreenwriterSuccessMessage"] = $"Screenwriter {screenwriterToConfirmDeletion.FirstName} " +
                $"{screenwriterToConfirmDeletion.LastName} deleted successfully!";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
