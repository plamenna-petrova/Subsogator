﻿using Data.DataModels.Entities;
using Subsogator.Web.Models.Actors.BindingModels;
using Subsogator.Web.Models.Actors.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Actors
{
    public interface IActorService
    {
        IEnumerable<AllActorsViewModel> GetAllActors();

        ActorDetailsViewModel GetActorDetails(string actorId);

        bool CreateActor(CreateActorBindingModel createActorBindingModel);

        EditActorBindingModel GetActorEditingDetails(string actorId);

        bool EditActor(EditActorBindingModel editActorBindingModel);

        DeleteActorViewModel GetActorDeletionDetails(string actorId);

        void DeleteActor(Actor actor);

        Actor FindActor(string actorId);
    }
}
