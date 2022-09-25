﻿using System;
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
using Subsogator.Common.GlobalConstants;
using Subsogator.Business.Services.FilmProductions;
using Subsogator.Web.Helpers;

namespace Subsogator.Web.Controllers
{
    public class ActorsController : BaseController
    {
        private readonly IActorService _actorService;

        private readonly IUnitOfWork _unitOfWork;

        public ActorsController(IActorService actorService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _actorService = actorService;
        }

        // GET: Actors
        public IActionResult Index(
            string sortOrder, 
            string currentFilter, 
            string searchTerm, 
            int? pageNumber
        )
        {
            IEnumerable<AllActorsViewModel> allActorsViewModel = _actorService
                    .GetAllActors();

            bool isAllActorsViewModelEmpty = allActorsViewModel.Count() == 0;

            if (isAllActorsViewModelEmpty)
            {
                return NotFound();
            }

            ViewData["CurrentSort"] = sortOrder;
            ViewData["ActorFirstNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "actor_first_name_descending"
                : "";
            ViewData["ActorLastNameSort"] = sortOrder == "actor_last_name_ascending"
                ? "actor_last_name_descending"
                : "actor_last_name_ascending";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["ActorSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allActorsViewModel = allActorsViewModel
                        .Where(avm =>
                            avm.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                            avm.LastName.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allActorsViewModel = sortOrder switch
            {
                "actor_first_name_descending" => allActorsViewModel
                        .OrderByDescending(avm => avm.FirstName),
                "actor_last_name_ascending" => allActorsViewModel
                        .OrderBy(avm => avm.LastName),
                "actor_last_name_descending" => allActorsViewModel
                        .OrderByDescending(avm => avm.LastName),
                _ => allActorsViewModel.OrderBy(avm => avm.FirstName)
            };

            int pageSize = 3;

            var paginatedList = PaginatedList<AllActorsViewModel>
                .Create(allActorsViewModel, pageNumber ?? 1, pageSize);
            
            return View(paginatedList);
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

            return View(_actorService.GetActorCreatingDetails());
        }

        // POST: Actors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            CreateActorBindingModel createActorBindingModel,
            string[] selectedFilmProductions
         )
        {
            if (!ModelState.IsValid)
            {
                return View(createActorBindingModel);
            }

            bool isNewActorCreated = _actorService
                    .CreateActor(createActorBindingModel, selectedFilmProductions);

            if (!isNewActorCreated)
            {
                TempData["ActorErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "actor", $"{createActorBindingModel.FirstName} " +
                        $"{createActorBindingModel.LastName}"
                    );

                return View(createActorBindingModel);
            }

            bool isNewActorSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewActorSavedToDatabase)
            {
                TempData["ActorErrorMessage"] = string.Format(
                    NotificationMessages.NewRecordFailedSaveErrorMessage, "actor"
                );

                return View(createActorBindingModel);
            }

            TempData["ActorSuccessMessage"] = string.Format(
                    NotificationMessages.RecordCreationSuccessMessage,
                    "Actor", $"{createActorBindingModel.FirstName} " +
                    $"{createActorBindingModel.LastName}"
                );

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
        public IActionResult Edit(
            EditActorBindingModel editActorBindingModel, 
            string[] selectedFilmProductions
        )
        {
            if (!ModelState.IsValid)
            {
                return View(editActorBindingModel);
            }

            bool isCurrentActorEdited = _actorService
                    .EditActor(editActorBindingModel, selectedFilmProductions);

            if (!isCurrentActorEdited)
            {
                TempData["ActorErrorMessage"] = string.Format(
                        NotificationMessages.ExistingRecordErrorMessage,
                        "actor", $"{editActorBindingModel.FirstName} " +
                        $"{editActorBindingModel.LastName}"
                    );

                return View(editActorBindingModel);
            }

            bool isCurrentActorUpdateSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isCurrentActorUpdateSavedToDatabase)
            {
                TempData["ActorErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedUpdateSaveErrorMessage,
                    "actor"
                  );

                return View(editActorBindingModel);
            }

            TempData["ActorSuccessMessage"] = string.Format(
                   NotificationMessages.RecordUpdateSuccessMessage,
                   "Actor", $"{editActorBindingModel.FirstName} " +
                   $"{editActorBindingModel.LastName}"
                );

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
                TempData["ActorErrorMessage"] = string.Format(
                    NotificationMessages.RecordFailedDeletionErrorMessage,
                    "actor"
                   ) + $"{actorToConfirmDeletion.FirstName} " +
                   $"{actorToConfirmDeletion.LastName}";

                return RedirectToAction(nameof(Delete));
            }

            TempData["ActorSuccessMessage"] = string.Format(
                    NotificationMessages.RecordDeletionSuccessMessage,
                    "Actor", $"{actorToConfirmDeletion.FirstName} " +
                    $"{actorToConfirmDeletion.LastName}"
                  );

            return RedirectToIndexActionInCurrentController();
        }
    }
}
