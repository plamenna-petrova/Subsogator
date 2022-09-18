using Data.DataAccess.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Subsogator.Web.Models.Actors.ViewModels;
using Data.DataAccess.Repositories.Interfaces;
using Subsogator.Web.Models.Actors.BindingModels;
using Data.DataModels.Entities;
using Subsogator.Business.Transactions.Interfaces;

namespace Subsogator.Business.Services.Actors
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public IEnumerable<AllActorsViewModel> GetAllActors()
        {
           List<AllActorsViewModel> allActors = _actorRepository
                .GetAllAsNoTracking()
                .Select(a => new AllActorsViewModel
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    LastName = a.LastName
                })
                .ToList();

            return allActors;
        }

        public ActorDetailsViewModel GetActorDetails(string actorId)
        {
            var singleActor = FindActor(actorId);

            if (singleActor is null)
            {
                return null;
            }

            var singleActorDetails = new ActorDetailsViewModel
            {
                Id = singleActor.Id,
                FirstName = singleActor.FirstName,
                LastName = singleActor.LastName,
                CreatedOn = singleActor.CreatedOn,
                ModifiedOn = singleActor.ModifiedOn
            };

            return singleActorDetails;
        }

        public bool CreateActor(CreateActorBindingModel createActorBindingModel)
        {
            Actor actorToCreate = new Actor 
            { 
                FirstName = createActorBindingModel.FirstName,
                LastName = createActorBindingModel.LastName,
            };

            var allActors = _actorRepository.GetAllAsNoTracking();

            if (_actorRepository.Exists(allActors, actorToCreate))
            {
                return false;
            }

            _actorRepository.Add(actorToCreate);

            return true;
        }

        public EditActorBindingModel GetActorEditingDetails(string actorId)
        {
            var actorToEdit = FindActor(actorId);

            if (actorToEdit is null)
            {
                return null;
            }

            var actorToEditDetails = new EditActorBindingModel
            {
                Id = actorToEdit.Id,
                FirstName = actorToEdit.FirstName,
                LastName = actorToEdit.LastName
            };

            return actorToEditDetails;
        }

        public bool EditActor(EditActorBindingModel editActorBindingModel)
        {
            var actorToUpdate = FindActor(editActorBindingModel.Id);

            actorToUpdate.FirstName = editActorBindingModel.FirstName;
            actorToUpdate.LastName = editActorBindingModel.LastName;

            var filteredActors = _actorRepository
                .GetAllAsNoTracking()
                .Where(a => !a.Id.Equals(actorToUpdate.Id))
                .AsQueryable();

            if (_actorRepository.Exists(filteredActors, actorToUpdate))
            {
                return false;
            }

            _actorRepository.Update(actorToUpdate);

            return true;
        }

        public DeleteActorViewModel GetActorDeletionDetails(string actorId)
        {
            var actorToDelete = FindActor(actorId);

            if (actorToDelete is null)
            {
                return null;
            }

            var actorToDeleteDetails = new DeleteActorViewModel
            {
                FirstName = actorToDelete.FirstName,
                LastName = actorToDelete.LastName
            };

            return actorToDeleteDetails;
        }

        public void DeleteActor(Actor actor)
        {
            _actorRepository.Delete(actor);
        }

        public Actor FindActor(string actorId)
        {
            return _actorRepository.GetById(actorId);
        } 
    }
}
