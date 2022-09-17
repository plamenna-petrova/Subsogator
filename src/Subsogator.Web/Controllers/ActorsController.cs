using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Actors;
using Subsogator.Web.Models.Actors.ViewModels;
using Subsogator.Web.Models.Actors.BindingModels;
using Subsogator.Business.Transactions.Interfaces;

namespace Subsogator.Web.Controllers
{
    public class ActorsController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _actorService = actorService;
        }

        // GET: Actors
        public ViewResult Index()
        {
            IEnumerable<AllActorsViewModel> allActorsViewModel = _actorService.GetAllActors();

            return View(allActorsViewModel);
        }

        // GET: Actors/Details/5
        public IActionResult Details(string id)
        {
            ActorDetailsViewModel actorDetailsViewModel = _actorService.GetActorDetails(id);

            if (actorDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(actorDetailsViewModel);
        }

        // GET: Actors/Create
        public ViewResult Create()
        {
            return View(new CreateActorBindingModel());
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateActorBindingModel createActorBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createActorBindingModel);
            }

            bool isNewActorCreated = _actorService.CreateActor(createActorBindingModel);

            if (!isNewActorCreated)
            {
                TempData["ActorErrorMessage"] = $"Error, the actor" +
                    $"{createActorBindingModel.FirstName}" +
                    $"{createActorBindingModel.LastName} already exists";
                return View(createActorBindingModel);
            }

            bool isNewActorSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewActorSavedToDatabase)
            {
                TempData["ActorErrorMessage"] = "Error, couldn't save the new" +
                    "actor record";
                return View(createActorBindingModel);
            }

            TempData["ActorSuccessMessage"] = $"Actor {createActorBindingModel.FirstName} " +
                $"{createActorBindingModel.LastName} created successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Actors/Edit/5
        public IActionResult Edit(string id)
        {
            EditActorBindingModel editActorBindingModel = _actorService
                .GetActorEditingDetails(id);

            if (editActorBindingModel == null)
            {
                return NotFound();
            }

            return View(editActorBindingModel);
        }

        // POST: Actors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditActorBindingModel editActorBindingModel)
        {
            if (!ModelState.IsValid)
            {
                return View(editActorBindingModel);
            }

            bool isCurrentActorEdited = _actorService.EditActor(editActorBindingModel);

            if (!isCurrentActorEdited)
            {
                TempData["ActorErrorMessage"] = $"Error, the actor " +
                $"{editActorBindingModel.FirstName} " +
                    $"{editActorBindingModel.LastName} already exists";
                return View(editActorBindingModel);
            }

            bool isCurrentActorUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentActorUpdateSavedToDatabase)
            {
                TempData["ActorErrorMessage"] = "Error, couldn't save the current" +
                    "actor update";
                return View(editActorBindingModel);
            }

            TempData["ActorSuccessMessage"] = $"Actor {editActorBindingModel.FirstName} " +
                $"{editActorBindingModel.LastName} saved successfully!";

            return RedirectToIndexActionInCurrentController();
        }

        // GET: Actors/Delete/5
        public IActionResult Delete(string id)
        {
            DeleteActorViewModel deleteActorViewModel = _actorService
                .GetActorDeletionDetails(id);

            if (deleteActorViewModel == null)
            {
                return NotFound();
            }

            return View(deleteActorViewModel);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDeletion(string id)
        {
            Actor actorToConfirmDeletion = _actorService.FindActor(id);

            _actorService.DeleteActor(actorToConfirmDeletion);

            bool isActorDeleted = _unitOfWork.CommitSaveChanges();

            if (!isActorDeleted)
            {
                TempData["ActorErrorMessage"] = "Error, couldn't delete the actor!";
                return RedirectToAction(nameof(Delete));
            }

            TempData["ActorSuccessMessage"] = $"Actor {actorToConfirmDeletion.FirstName} " +
                $"{actorToConfirmDeletion.LastName} deleted successfully!";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
