using Data.DataModels.Entities;
using Subsogator.Web.Models.Actors.BindingModels;
using Subsogator.Web.Models.Actors.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Actors
{
    public interface IActorService
    {
        IEnumerable<AllActorsViewModel> GetAllActors();

        ActorDetailsViewModel GetActorDetails(string actorId);

        CreateActorBindingModel GetActorCreatingDetails();

        bool CreateActor(CreateActorBindingModel createActorBindingModel, string[] selectedFilmProductions);

        EditActorBindingModel GetActorEditingDetails(string actorId);

        bool EditActor(EditActorBindingModel editActorBindingModel, string[] selectedFilmProductions);

        DeleteActorViewModel GetActorDeletionDetails(string actorId);

        void DeleteActor(Actor actor);

        Actor FindActor(string actorId);
    }
}
