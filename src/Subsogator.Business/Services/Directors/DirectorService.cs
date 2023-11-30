using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.Directors.BindingModels;
using Subsogator.Web.Models.Directors.ViewModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace Subsogator.Business.Services.Directors
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;

        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IFilmProductionDirectorRepository _filmProductionDirectorRepository;

        public DirectorService(
            IDirectorRepository directorRepository,
            IFilmProductionRepository filmProductionRepository,
            IFilmProductionDirectorRepository filmProductionDirectorRepository
        )
        {
            _directorRepository = directorRepository;
            _filmProductionRepository = filmProductionRepository;
            _filmProductionDirectorRepository = filmProductionDirectorRepository;
        }

        public IEnumerable<AllDirectorsViewModel> GetAllDirectors()
        {
            List<AllDirectorsViewModel> allDirectors = _directorRepository
                 .GetAllAsNoTracking()
                    .OrderBy(d => d.FirstName)
                        .Select(d => new AllDirectorsViewModel
                        {
                            Id = d.Id,
                            FirstName = d.FirstName,
                            LastName = d.LastName,
                            RelatedFilmProductions = d.FilmProductionDirectors
                                .Where(fpd => fpd.DirectorId == d.Id)
                                .Select(fpd => new FilmProductionConciseInformationViewModel
                                {
                                    Title = fpd.FilmProduction.Title
                                })
                        })
                        .ToList();

            return allDirectors;
        }

        public DirectorDetailsViewModel GetDirectorDetails(string directorId)
        {
            var singleDirector = _directorRepository
                    .GetAllByCondition(d => d.Id == directorId)
                         .FirstOrDefault();

            if (singleDirector is null)
            {
                return null;
            }

            var singleDirectorDetails = new DirectorDetailsViewModel
            {
                Id = singleDirector.Id,
                FirstName = singleDirector.FirstName,
                LastName = singleDirector.LastName,
                CreatedOn = singleDirector.CreatedOn,
                CreatedBy = singleDirector.CreatedBy,
                ModifiedOn = singleDirector.ModifiedOn,
                ModifiedBy = singleDirector.ModifiedBy,
                RelatedFilmProductions = singleDirector.FilmProductionDirectors
                    .Where(fpd => fpd.DirectorId == singleDirector.Id)
                    .Select(fpd => new FilmProductionDetailedInformationViewModel
                    {
                        Title = fpd.FilmProduction.Title,
                        Duration = fpd.FilmProduction.Duration,
                        ReleaseDate = fpd.FilmProduction.ReleaseDate
                    })
            };

            return singleDirectorDetails;
        }

        public CreateDirectorBindingModel GetDirectorCreatingDetails()
        {
            var director = new Director();

            var directorCreationDetails = new CreateDirectorBindingModel
            {
                FirstName = director.FirstName,
                LastName = director.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(director)
            };

            return directorCreationDetails;
        }

        public bool CreateDirector(
            CreateDirectorBindingModel createDirectorBindingModel,
            string[] selectedFilmProductions,
            string currentUserName
        )
        {
            Director directorToCreate = new Director
            {
                FirstName = createDirectorBindingModel.FirstName,
                LastName = createDirectorBindingModel.LastName
            };

            var allDirectors = _directorRepository.GetAllAsNoTracking();

            if (_directorRepository.Exists(allDirectors, directorToCreate))
            {
                return false;
            }

            if (selectedFilmProductions != null)
            {
                foreach (var filmProductionId in selectedFilmProductions)
                {
                    var filmProductionActorToAdd = new FilmProductionDirector
                    {
                        FilmProductionId = filmProductionId,
                        DirectorId = directorToCreate.Id
                    };

                    directorToCreate.FilmProductionDirectors.Add(filmProductionActorToAdd);
                }
            }

            directorToCreate.CreatedBy = currentUserName;   

            _directorRepository.Add(directorToCreate);

            return true;
        }

        public EditDirectorBindingModel GetDirectorEditingDetails(string directorId)
        {
            var directorToEdit = _directorRepository
                    .GetAllByCondition(d=> d.Id == directorId)
                        .Include(d => d.FilmProductionDirectors)
                            .ThenInclude(fpd => fpd.FilmProduction)
                                .FirstOrDefault();

            if (directorToEdit is null)
            {
                return null;
            }

            var directorToEditDetails = new EditDirectorBindingModel
            {
                Id = directorToEdit.Id,
                FirstName = directorToEdit.FirstName,
                LastName = directorToEdit.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(directorToEdit)
            };

            return directorToEditDetails;
        }

        public bool EditDirector(
            EditDirectorBindingModel editDirectorBindingModel,
            string[] selectedFilmProductions,
            string currentUserName
        )
        {
            var directorToUpdate = _directorRepository
                    .GetAllByCondition(d => d.Id == editDirectorBindingModel.Id)
                        .Include(d => d.FilmProductionDirectors)
                            .ThenInclude(fpd => fpd.FilmProduction)
                                .FirstOrDefault();

            directorToUpdate.FirstName = editDirectorBindingModel.FirstName;
            directorToUpdate.LastName = editDirectorBindingModel.LastName;

            var filteredActors = _directorRepository
                .GetAllAsNoTracking()
                    .Where(d => !d.Id.Equals(directorToUpdate.Id))
                        .AsQueryable();

            if (_directorRepository.Exists(filteredActors, directorToUpdate))
            {
                return false;
            }

            directorToUpdate.ModifiedBy = currentUserName;

            _directorRepository.Update(directorToUpdate);

            UpdateFilmProductionDirectorsByDirector(selectedFilmProductions, directorToUpdate);

            return true;
        }

        public DeleteDirectorViewModel GetDirectorDeletionDetails(string directorId)
        {
            var directorToDelete = FindDirector(directorId);

            if (directorToDelete is null)
            {
                return null;
            }

            var directorToDeleteDetails = new DeleteDirectorViewModel
            {
                FirstName = directorToDelete.FirstName,
                LastName = directorToDelete.LastName
            };

            return directorToDeleteDetails;
        }

        public void DeleteDirector(Director director)
        {
            var filmProductionDirectorsByDirector = _filmProductionDirectorRepository
                    .GetAllByCondition(fpd => fpd.DirectorId == director.Id)
                        .ToArray();

            if (filmProductionDirectorsByDirector.Any()) 
            {
                _filmProductionDirectorRepository.DeleteRange(filmProductionDirectorsByDirector);
            }

            _directorRepository.Delete(director);
        }

        public Director FindDirector(string directorId)
        {
            return _directorRepository.GetById(directorId);
        }

        private List<AssignedFilmProductionDataViewModel> PopulateAssignedFilmProductionData(
           Director director
        )
        {
            var allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .ToList();

            var filmProductionsOfADirector = new HashSet<string>(director.FilmProductionDirectors
                .Select(fpd => fpd.FilmProduction.Id));

            var assignedFilmProductionDataViewModel =
                    new List<AssignedFilmProductionDataViewModel>();

            foreach (var filmProduction in allFilmProductions)
            {
                assignedFilmProductionDataViewModel.Add(new AssignedFilmProductionDataViewModel
                {
                    FilmProductionId = filmProduction.Id,
                    Title = filmProduction.Title,
                    IsAssigned = filmProductionsOfADirector.Contains(filmProduction.Id)
                });
            }

            return assignedFilmProductionDataViewModel;
        }

        private void UpdateFilmProductionDirectorsByDirector(
           string[] selectedFilmProductions,
           Director director
        )
        {
            if (selectedFilmProductions == null)
            {
                director.FilmProductionDirectors = new List<FilmProductionDirector>();
                return;
            }

            var selectedFilmProductionsIds = new HashSet<string>(selectedFilmProductions);

            var filmProductionsOfADirector = new HashSet<string>(
                    director.FilmProductionDirectors.Select(fpd => fpd.FilmProduction.Id)
                );

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking();

            foreach (var filmProduction in allFilmProductions)
            {
                if (selectedFilmProductionsIds.Contains(filmProduction.Id))
                {
                    if (!filmProductionsOfADirector.Contains(filmProduction.Id))
                    {
                        director.FilmProductionDirectors.Add(new FilmProductionDirector
                        {
                            FilmProductionId = filmProduction.Id,
                            DirectorId = director.Id
                        });
                    }
                }
                else
                {
                    if (filmProductionsOfADirector.Contains(filmProduction.Id))
                    {
                        FilmProductionDirector filmProductionDirectorToRemove =
                            director.FilmProductionDirectors
                                    .FirstOrDefault(fpd =>
                                        fpd.FilmProductionId == filmProduction.Id
                                    );

                        _filmProductionDirectorRepository.Delete(filmProductionDirectorToRemove);
                    }
                }
            }
        }
    }
}

