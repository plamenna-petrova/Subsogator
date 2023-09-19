using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Mapping;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Subsogator.Business.Services.Screenwriters
{
    public class ScreenwriterService: IScreenwriterService
    {
        private readonly IScreenwriterRepository _screenwriterRepository;

        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IFilmProductionScreenwriterRepository _filmProductionScreenwriterRepository;

        public ScreenwriterService(
            IScreenwriterRepository screenwriterRepository,
            IFilmProductionRepository filmProductionRepository,
            IFilmProductionScreenwriterRepository filmProductionScreenwriterRepository
        )
        {
            _screenwriterRepository = screenwriterRepository;
            _filmProductionRepository = filmProductionRepository;
            _filmProductionScreenwriterRepository = filmProductionScreenwriterRepository;
        }

        public IEnumerable<AllScreenwritersViewModel> GetAllScreenwriters()
        {
            List<AllScreenwritersViewModel> allScreenwriters = _screenwriterRepository
                 .GetAllAsNoTracking()
                    .OrderBy(s => s.FirstName)
                        .Select(s => new AllScreenwritersViewModel
                        {
                            Id = s.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            RelatedFilmProductions = s.FilmProductionScreenwriters
                                .Where(fps => fps.ScreenwriterId == s.Id)
                                .Select(fps => new FilmProductionConciseInformationViewModel
                                {
                                    Title = fps.FilmProduction.Title
                                })
                        })
                        .ToList();

            return allScreenwriters;
        }

        public ScreenwriterDetailsViewModel GetScreenwriterDetails(string screenwriterId)
        {
            var singleScreenwriter = _screenwriterRepository
                    .GetAllByCondition(s => s.Id == screenwriterId)
                         .FirstOrDefault();

            if (singleScreenwriter is null)
            {
                return null;
            }

            var singleScreenwriterDetails = new ScreenwriterDetailsViewModel
            {
                Id = singleScreenwriter.Id,
                FirstName = singleScreenwriter.FirstName,
                LastName = singleScreenwriter.LastName,
                CreatedOn = singleScreenwriter.CreatedOn,
                ModifiedOn = singleScreenwriter.ModifiedOn,
                RelatedFilmProductions = singleScreenwriter.FilmProductionScreenwriters
                    .Where(fps => fps.ScreenwriterId == singleScreenwriter.Id)
                    .Select(fps => new FilmProductionDetailedInformationViewModel
                    {
                        Title = fps.FilmProduction.Title,
                        Duration = fps.FilmProduction.Duration,
                        ReleaseDate = fps.FilmProduction.ReleaseDate
                    })
            };

            return singleScreenwriterDetails;
        }

        public CreateScreenwriterBindingModel GetScreenwriterCreatingDetails()
        {
            var screenwriter = new Screenwriter();

            var screenwriterCreationDetails = new CreateScreenwriterBindingModel
            {
                FirstName = screenwriter.FirstName,
                LastName = screenwriter.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(screenwriter)
            };

            return screenwriterCreationDetails;
        }

        public bool CreateScreenwriter(
            CreateScreenwriterBindingModel createScreenwriterBindingModel,
            string[] selectedFilmProductions
        )
        {
            Screenwriter screenwriterToCreate = new Screenwriter
            {
                FirstName = createScreenwriterBindingModel.FirstName,
                LastName = createScreenwriterBindingModel.LastName
            };

            var allScreenwriters = _screenwriterRepository.GetAllAsNoTracking();

            if (_screenwriterRepository.Exists(allScreenwriters, screenwriterToCreate))
            {
                return false;
            }

            if (selectedFilmProductions != null)
            {
                foreach (var filmProductionId in selectedFilmProductions)
                {
                    var filmProductionActorToAdd = new FilmProductionScreenwriter
                    {
                        FilmProductionId = filmProductionId,
                        ScreenwriterId = screenwriterToCreate.Id
                    };

                    screenwriterToCreate.FilmProductionScreenwriters.Add(filmProductionActorToAdd);
                }
            }

            _screenwriterRepository.Add(screenwriterToCreate);

            return true;
        }

        public EditScreenwriterBindingModel GetScreenwriterEditingDetails(string screenwriterId)
        {
            var screenwriterToEdit = _screenwriterRepository
                    .GetAllByCondition(s => s.Id == screenwriterId)
                        .Include(s => s.FilmProductionScreenwriters)
                            .ThenInclude(fps => fps.FilmProduction)
                                .FirstOrDefault();

            if (screenwriterToEdit is null)
            {
                return null;
            }

            var screenwriterToEditDetails = new EditScreenwriterBindingModel
            {
                Id = screenwriterToEdit.Id,
                FirstName = screenwriterToEdit.FirstName,
                LastName = screenwriterToEdit.LastName,
                AssignedFilmProductions = PopulateAssignedFilmProductionData(screenwriterToEdit)
            };

            return screenwriterToEditDetails;
        }

        public bool EditScreenwriter(
            EditScreenwriterBindingModel editScreenwriterBindingModel,
            string[] selectedFilmProductions
        )
        {
            var screenwriterToUpdate = _screenwriterRepository
                    .GetAllByCondition(s => s.Id == editScreenwriterBindingModel.Id)
                        .Include(s => s.FilmProductionScreenwriters)
                            .ThenInclude(fps => fps.FilmProduction)
                                .FirstOrDefault();

            screenwriterToUpdate.FirstName = editScreenwriterBindingModel.FirstName;
            screenwriterToUpdate.LastName = editScreenwriterBindingModel.LastName;

            var filteredActors = _screenwriterRepository
                .GetAllAsNoTracking()
                    .Where(s => !s.Id.Equals(screenwriterToUpdate.Id))
                        .AsQueryable();

            if (_screenwriterRepository.Exists(filteredActors, screenwriterToUpdate))
            {
                return false;
            }

            _screenwriterRepository.Update(screenwriterToUpdate);

            UpdateFilmProductionScreenwritersByScreenwriter(
                selectedFilmProductions, 
                screenwriterToUpdate
            );

            return true;
        }

        public DeleteScreenwriterViewModel GetScreenwriterDeletionDetails(string screenwriterId)
        {
            var screenwriterToDelete = FindScreenwriter(screenwriterId);

            if (screenwriterToDelete is null)
            {
                return null;
            }

            var screenwriterToDeleteDetails = new DeleteScreenwriterViewModel
            {
                FirstName = screenwriterToDelete.FirstName,
                LastName = screenwriterToDelete.LastName
            };

            return screenwriterToDeleteDetails;
        }

        public void DeleteScreenwriter(Screenwriter screenwriter)
        {
            var filmProductionScreenwritersByScreenwriter = _filmProductionScreenwriterRepository
                    .GetAllByCondition(fps => fps.ScreenwriterId == screenwriter.Id)
                        .ToArray();

            _filmProductionScreenwriterRepository.DeleteRange(filmProductionScreenwritersByScreenwriter);

            _screenwriterRepository.Delete(screenwriter);
        }

        public Screenwriter FindScreenwriter(string screenwriterId)
        {
            return _screenwriterRepository.GetById(screenwriterId);
        }

        private List<AssignedFilmProductionDataViewModel> PopulateAssignedFilmProductionData(
           Screenwriter screenwriter
        )
        {
            var allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .ToList();

            var filmProductionsOfAScreenwriter = new HashSet<string>(screenwriter.FilmProductionScreenwriters
                .Select(fs => fs.FilmProduction.Id));

            var assignedFilmProductionDataViewModel =
                    new List<AssignedFilmProductionDataViewModel>();

            foreach (var filmProduction in allFilmProductions)
            {
                assignedFilmProductionDataViewModel.Add(new AssignedFilmProductionDataViewModel
                {
                    FilmProductionId = filmProduction.Id,
                    Title = filmProduction.Title,
                    IsAssigned = filmProductionsOfAScreenwriter.Contains(filmProduction.Id)
                });
            }

            return assignedFilmProductionDataViewModel;
        }

        private void UpdateFilmProductionScreenwritersByScreenwriter(
           string[] selectedFilmProductions,
           Screenwriter screenwriter
       )
        {
            if (selectedFilmProductions == null)
            {
                screenwriter.FilmProductionScreenwriters = new List<FilmProductionScreenwriter>();
                return;
            }

            var selectedFilmProductionsIds = new HashSet<string>(selectedFilmProductions);

            var filmProductionsOfAScreenwriter = new HashSet<string>(
                    screenwriter.FilmProductionScreenwriters.Select(fps => fps.FilmProduction.Id)
                );

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking();

            foreach (var filmProduction in allFilmProductions)
            {
                if (selectedFilmProductionsIds.Contains(filmProduction.Id))
                {
                    if (!filmProductionsOfAScreenwriter.Contains(filmProduction.Id))
                    {
                        screenwriter.FilmProductionScreenwriters.Add(new FilmProductionScreenwriter
                        {
                            FilmProductionId = filmProduction.Id,
                            ScreenwriterId = screenwriter.Id
                        });
                    }
                }
                else
                {
                    if (filmProductionsOfAScreenwriter.Contains(filmProduction.Id))
                    {
                        FilmProductionScreenwriter filmProductionScreenwriterToRemove =
                            screenwriter.FilmProductionScreenwriters
                                    .FirstOrDefault(fps =>
                                        fps.FilmProductionId == filmProduction.Id
                                    );

                        _filmProductionScreenwriterRepository
                            .Delete(filmProductionScreenwriterToRemove);
                    }
                }
            }
        }
    }
}
