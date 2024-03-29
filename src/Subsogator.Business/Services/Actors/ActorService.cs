﻿using System.Collections.Generic;
using System.Linq;
using Subsogator.Web.Models.Actors.ViewModels;
using Data.DataAccess.Repositories.Interfaces;
using Subsogator.Web.Models.Actors.BindingModels;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Mapping;

namespace Subsogator.Business.Services.Actors
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IFilmProductionActorRepository _filmProductionActorRepository;

        public ActorService(
            IActorRepository actorRepository, 
            IFilmProductionRepository filmProductionRepository,
            IFilmProductionActorRepository filmProductionActorRepository
        )
        {
            _actorRepository = actorRepository;
            _filmProductionRepository = filmProductionRepository;
            _filmProductionActorRepository = filmProductionActorRepository;
        }

        public IEnumerable<AllActorsViewModel> GetAllActors()
        {
            List<AllActorsViewModel> allActors = _actorRepository
                .GetAllAsNoTracking()
                        .Select(a => new AllActorsViewModel
                        {
                            Id = a.Id,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            RelatedFilmProductions = a.FilmProductionActors
                                .Where(fpa => fpa.ActorId == a.Id)
                                .OrderBy(fpa => fpa.FilmProduction.Title)
                                .Select(fpa => new FilmProductionConciseInformationViewModel
                                {
                                    Title = fpa.FilmProduction.Title
                                })
                        })
                        .ToList();

            return allActors;
        }

        public ActorDetailsViewModel GetActorDetails(string actorId)
        {
            var singleActor = _actorRepository
                  .GetAllByCondition(a => a.Id == actorId)
                       .FirstOrDefault();

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking();

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
                CreatedBy = singleActor.CreatedBy,
                ModifiedOn = singleActor.ModifiedOn,
                ModifiedBy = singleActor.ModifiedBy,
                RelatedFilmProductions = singleActor.FilmProductionActors
                    .Where(fpa => fpa.ActorId == singleActor.Id)
                    .OrderBy(fpa => fpa.FilmProduction.Title)
                    .Select(fpa => new FilmProductionDetailedInformationViewModel
                    {
                        Title = fpa.FilmProduction.Title,
                        Duration = fpa.FilmProduction.Duration,
                        ReleaseDate = fpa.FilmProduction.ReleaseDate
                    })
                    .ToList()
            };

            return singleActorDetails;
        }

        public CreateActorBindingModel GetActorCreatingDetails()
        {
            var actor = new Actor();

            var actorCreationDetails = new CreateActorBindingModel
            {
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(actor)
            };

            return actorCreationDetails;
        }

        public bool CreateActor(
            CreateActorBindingModel createActorBindingModel, 
            string[] selectedFilmProductions,
            string currentUserName
        )
        {
            Actor actorToCreate = new Actor
            {
                FirstName = createActorBindingModel.FirstName,
                LastName = createActorBindingModel.LastName
            };

            var allActors = _actorRepository.GetAllAsNoTracking();

            if (_actorRepository.Exists(allActors, actorToCreate))
            {
                return false;
            }

            if (selectedFilmProductions != null)
            {
                foreach (var filmProductionId in selectedFilmProductions)
                {
                    var filmProductionActorToAdd = new FilmProductionActor
                    {
                        FilmProductionId = filmProductionId,
                        ActorId = actorToCreate.Id
                    };

                    actorToCreate.FilmProductionActors.Add(filmProductionActorToAdd);
                }
            }

            actorToCreate.CreatedBy = currentUserName;

            _actorRepository.Add(actorToCreate);

            return true;
        }

        public EditActorBindingModel GetActorEditingDetails(string actorId)
        {
            var actorToEdit = _actorRepository
                    .GetAllByCondition(a => a.Id == actorId)
                        .Include(a => a.FilmProductionActors)
                            .ThenInclude(fpa => fpa.FilmProduction)
                                .FirstOrDefault();

            if (actorToEdit is null)
            {
                return null;
            }

            var actorToEditDetails = new EditActorBindingModel
            {
                Id = actorToEdit.Id,
                FirstName = actorToEdit.FirstName,
                LastName = actorToEdit.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(actorToEdit)
            };

            return actorToEditDetails;
        }

        public bool EditActor(
            EditActorBindingModel editActorBindingModel, 
            string[] selectedFilmProductions,
            string currentUserName
        )
        {
            var actorToUpdate = _actorRepository
                    .GetAllByCondition(a => a.Id == editActorBindingModel.Id)
                        .Include(a => a.FilmProductionActors)
                            .ThenInclude(fpa => fpa.FilmProduction)
                                .FirstOrDefault();

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

            actorToUpdate.ModifiedBy = currentUserName;

            _actorRepository.Update(actorToUpdate);

            UpdateFilmProductionActorsByActor(selectedFilmProductions, actorToUpdate);

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
            var filmProductionActorsByActor = _filmProductionActorRepository
                    .GetAllByCondition(fpa => fpa.ActorId == actor.Id)
                        .ToArray();

            if (filmProductionActorsByActor.Any())
            {
                _filmProductionActorRepository.DeleteRange(filmProductionActorsByActor);
            }

            _actorRepository.Delete(actor);
        }

        public Actor FindActor(string actorId)
        {
            return _actorRepository.GetById(actorId);
        }

        private List<AssignedFilmProductionDataViewModel> PopulateAssignedFilmProductionData(
            Actor actor
         )
        {
            var allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .ToList();

            var filmProductionsOfAnActor = new HashSet<string>(actor.FilmProductionActors
                .Select(fpa => fpa.FilmProduction.Id));

            var assignedFilmProductionDataViewModel = 
                    new List<AssignedFilmProductionDataViewModel>();

            foreach (var filmProduction in allFilmProductions)
            {
                assignedFilmProductionDataViewModel.Add(new AssignedFilmProductionDataViewModel
                {
                    FilmProductionId = filmProduction.Id,
                    Title = filmProduction.Title,
                    IsAssigned = filmProductionsOfAnActor.Contains(filmProduction.Id)
                });
            }

            return assignedFilmProductionDataViewModel;
        }

        private void UpdateFilmProductionActorsByActor(
            string[] selectedFilmProductions, 
            Actor actor
        )
        {
            if (selectedFilmProductions == null)
            {
                actor.FilmProductionActors = new List<FilmProductionActor>();
                return;
            }

            var selectedFilmProductionsIds = new HashSet<string>(selectedFilmProductions);

            var filmProductionsOfAnActor = new HashSet<string>(
                    actor.FilmProductionActors.Select(fpa => fpa.FilmProduction.Id)
                );

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking();

            foreach (var filmProduction in allFilmProductions)
            {
                if (selectedFilmProductionsIds.Contains(filmProduction.Id))
                {
                    if (!filmProductionsOfAnActor.Contains(filmProduction.Id))
                    {
                        actor.FilmProductionActors.Add(new FilmProductionActor
                        {
                            FilmProductionId = filmProduction.Id,
                            ActorId = actor.Id
                        });
                    }
                } 
                else
                {
                    if (filmProductionsOfAnActor.Contains(filmProduction.Id))
                    {
                        FilmProductionActor filmProductionActorToRemove = 
                            actor.FilmProductionActors
                                    .FirstOrDefault(fpa => 
                                        fpa.FilmProductionId == filmProduction.Id
                                    );

                        _filmProductionActorRepository.Delete(filmProductionActorToRemove);
                    }
                }
            }
        }
    }
}
