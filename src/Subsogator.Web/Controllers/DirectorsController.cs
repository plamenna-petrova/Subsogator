using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Business.Services.Directors;
using Subsogator.Web.Models.Directors;
using Subsogator.Web.Models.Directors.ViewModels;
using Subsogator.Web.Models.Directors.BindingModels;

namespace Subsogator.Web.Controllers
{
    public class DirectorsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IDirectorService _directorService;

        public DirectorsController(IDirectorService directorService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _directorService = directorService;
        }

        // GET: Directors
        public ViewResult Index()
        {
            IEnumerable<AllDirectorsViewModel> allDirectorsViewModel = _directorService.GetAllDirectors();

            return View(allDirectorsViewModel);
        }

        // GET: Directors/Details/5
        public IActionResult Details(string id)
        {
            DirectorDetailsViewModel directorDetailsViewModel = _directorService.GetDirectorDetails(id);

            if (directorDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(directorDetailsViewModel);
        }

        // GET: Directors/Create
        public ViewResult Create()
        {
            return View(new CreateDirectorBindingModel());
        }

        // POST: Directors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateDirectorBindingModel createDirectorBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createDirectorBindingModel);
            }

            bool isNewDirectorCreated = _directorService.CreateDirector(createDirectorBindingModel);

            if (!isNewDirectorCreated)
            {
                TempData["DirectorErrorMessage"] = $"Error, the director" +
                    $"{createDirectorBindingModel.FirstName}" +
                    $"{createDirectorBindingModel.LastName} already exists";
                return View(createDirectorBindingModel);
            }

            bool isNewDirectorSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewDirectorSavedToDatabase)
            {
                TempData["DirectorErrorMessage"] = "Error, couldn't save the new" +
                    "director record";
                return View(createDirectorBindingModel);
            }

            TempData["DirectorSuccessMessage"] = $"Director {createDirectorBindingModel.FirstName} " +
                $"{createDirectorBindingModel.LastName} created successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Directors/Edit/5
        public IActionResult Edit(string id)
        {
            EditDirectorBindingModel editDirectorBindingModel = _directorService
                .GetDirectorEditingDetails(id);

            if (editDirectorBindingModel == null)
            {
                return NotFound();
            }

            return View(editDirectorBindingModel);
        }

        // POST: Directors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditDirectorBindingModel editDirectorBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editDirectorBindingModel);
            }

            bool isCurrentDirectorEdited = _directorService.EditDirector(editDirectorBindingModel);

            if (!isCurrentDirectorEdited)
            {
                TempData["DirectorErrorMessage"] = $"Error, the director " +
                $"{editDirectorBindingModel.FirstName} " +
                    $"{editDirectorBindingModel.LastName} already exists";
                return View(editDirectorBindingModel);
            }

            bool isCurrentDirectorUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentDirectorUpdateSavedToDatabase)
            {
                TempData["DirectorErrorMessage"] = "Error, couldn't save the current" +
                    "director update";
                return View(editDirectorBindingModel);
            }

            TempData["DirectorSuccessMessage"] = $"Director {editDirectorBindingModel.FirstName} " +
                $"{editDirectorBindingModel.LastName} saved successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Directors/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteDirectorViewModel deleteDirectorViewModel = _directorService
                .GetDirectorDeletionDetails(id);

            if (deleteDirectorViewModel == null)
            {
                return NotFound();
            }

            return View(deleteDirectorViewModel);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Director directorToConfirmDeletion = _directorService.FindDirector(id);

            _directorService.DeleteDirector(directorToConfirmDeletion);

            bool isDirectorDeleted = _unitOfWork.CommitSaveChanges();

            if (!isDirectorDeleted)
            {
                TempData["DirectorErrorMessage"] = "Error, couldn't delete the director!";
                return RedirectToAction(nameof(Delete));
            }

            TempData["DirectorSuccessMessage"] = $"Director {directorToConfirmDeletion.FirstName} " +
                $"{directorToConfirmDeletion.LastName} deleted successfully!";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
