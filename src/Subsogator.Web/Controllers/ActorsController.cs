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
using Subsogator.Common.GlobalConstants;
using Subsogator.Business.Services.FilmProductions;
using Subsogator.Web.Helpers;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Index(
            string sortOrder, 
            string currentFilter, 
            string searchTerm,
            int? pageSize,
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
                        .Where(aavm =>
                            aavm.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                            aavm.LastName.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allActorsViewModel = sortOrder switch
            {
                "actor_first_name_descending" => allActorsViewModel
                        .OrderByDescending(aavm => aavm.FirstName),
                "actor_last_name_ascending" => allActorsViewModel
                        .OrderBy(aavm => aavm.LastName),
                "actor_last_name_descending" => allActorsViewModel
                        .OrderByDescending(aavm => aavm.LastName),
                _ => allActorsViewModel.OrderBy(aavm => aavm.FirstName)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var paginatedList = PaginatedList<AllActorsViewModel>
                .Create(allActorsViewModel, pageNumber ?? 1, (int) pageSize);
            
            return View(paginatedList);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public IActionResult Details(string id)
        {
            ActorDetailsViewModel actorDetailsViewModel = _actorService.GetActorDetails(id);

            if (actorDetailsViewModel == null)
            {
                return NotFound();
            }

            return View(actorDetailsViewModel);
        }

        [Authorize(Roles = "Administrator, Editor")]
        public ViewResult Create()
        {
            return View(_actorService.GetActorCreatingDetails());
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
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

        [Authorize(Roles = "Administrator, Editor")]
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

        [HttpPost]
        [Authorize(Roles = "Administrator, Editor")]
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

        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = IdentityConstants.AdministratorRoleName)]
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
