using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Web.Models.FilmProductions.BindingModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Languages.ViewModels;
using Subsogator.Web.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Subsogator.Business.Services.FilmProductions
{
    public class FilmProductionService : IFilmProductionService
    {
        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IGenreRepository _genreRepository;

        private readonly IActorRepository _actorRepository;

        private readonly IDirectorRepository _directorRepository;

        private readonly IScreenwriterRepository _screenwriterRepository;

        private readonly IFilmProductionGenreRepository _filmProductionGenreRepository;

        private readonly IFilmProductionActorRepository _filmProductionActorRepository;

        private readonly IFilmProductionDirectorRepository _filmProductonDirectorRepository;

        private readonly IFilmProductionScreenwriterRepository _filmProductionScreenwriterRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilmProductionService(
            IFilmProductionRepository filmProductionRepository,
            IGenreRepository genreRepository,
            IActorRepository actorRepository,
            IDirectorRepository directorRepository,
            IScreenwriterRepository screenwriterRepository,
            IFilmProductionGenreRepository filmProductionGenreRepository,
            IFilmProductionActorRepository filmProductionActorRepository,
            IFilmProductionDirectorRepository filmProductionDirectorRepository,
            IFilmProductionScreenwriterRepository filmProductionScreenwriterRepository,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _filmProductionRepository = filmProductionRepository;
            _genreRepository = genreRepository;
            _actorRepository = actorRepository;
            _directorRepository = directorRepository;
            _screenwriterRepository = screenwriterRepository;
            _filmProductionGenreRepository = filmProductionGenreRepository;
            _filmProductionActorRepository = filmProductionActorRepository;
            _filmProductonDirectorRepository = filmProductionDirectorRepository;
            _filmProductionScreenwriterRepository = filmProductionScreenwriterRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<FilmProduction> GetAllFilmProductions()
        {
            return _filmProductionRepository.GetAllAsNoTracking().ToList();
        }

        public IEnumerable<AllFilmProductionsViewModel> GetAllFilmProductionsWithRelatedData()
        {
            List<AllFilmProductionsViewModel> allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .Select(fp => new AllFilmProductionsViewModel
                    {
                        Id = fp.Id,
                        Title = fp.Title,
                        Duration = fp.Duration,
                        ReleaseDate = fp.ReleaseDate,
                        RelatedCountry = new CountryConciseInformationViewModel
                        {
                            Name = fp.Country.Name
                        },
                        RelatedLanguage = new LanguageConciseInformationViewModel
                        {
                            Name = fp.Language.Name
                        }
                    })
                    .OrderBy(afpvm => afpvm.Title)
                        .ThenBy(afpvm => afpvm.RelatedCountry.Name)
                            .ThenByDescending(afpvm => afpvm.RelatedLanguage.Name)
                                .ToList();

            return allFilmProductions;
        }

        public FilmProductionFullDetailsViewModel GetFilmProductionDetails(
            string filmProductionId
        )
        {
            var singleFilmProduction = _filmProductionRepository
                    .GetAllByCondition(fp => fp.Id == filmProductionId)
                        .Include(fp => fp.Country)
                        .Include(fp => fp.Language)
                        .Include(fp => fp.FilmProductionGenres)
                            .ThenInclude(fg => fg.Genre)
                        .Include(fp => fp.FilmProductionActors)
                            .ThenInclude(fa => fa.Actor)
                        .Include(fp => fp.FilmProductionDirectors)
                            .ThenInclude(fd => fd.Director)
                        .Include(fp => fp.FilmProductionScreenwriters)
                            .ThenInclude(fs => fs.Screenwriter)
                        .FirstOrDefault();

            if (singleFilmProduction is null)
            {
                return null;
            }

            var singleFilmProductionDetails = new FilmProductionFullDetailsViewModel
            {
                Id = singleFilmProduction.Id,
                Title = singleFilmProduction.Title,
                Duration = singleFilmProduction.Duration,
                ReleaseDate = singleFilmProduction.ReleaseDate,
                PlotSummary = singleFilmProduction.PlotSummary,
                CountryName = singleFilmProduction.Country.Name,
                LanguageName = singleFilmProduction.Language.Name,
                ImageName = singleFilmProduction.ImageName,
                RelatedGenres = singleFilmProduction.FilmProductionGenres
                                    .Select(fg => fg.Genre.Name)
                                    .ToList(),
                RelatedActors = singleFilmProduction.FilmProductionActors
                                    .Select(fa => new Tuple<string, string>(
                                        fa.Actor.FirstName,
                                        fa.Actor.LastName)
                                    )
                                    .ToList(),
                RelatedDirectors = singleFilmProduction.FilmProductionDirectors
                                    .Select(fd => new Tuple<string, string>(
                                            fd.Director.FirstName,
                                            fd.Director.LastName
                                        ))
                                    .ToList(),
                RelatedScreenwriters = singleFilmProduction.FilmProductionScreenwriters
                                    .Select(fs => new Tuple<string, string>(
                                            fs.Screenwriter.FirstName,
                                            fs.Screenwriter.LastName
                                        ))
                                    .ToList()
            };

            return singleFilmProductionDetails;
        }

        public CreateFilmProductionBindingModel GetFilmProductionCreatingDetails()
        {
            var filmProduction = new FilmProduction();

            var filmProductionRelatedEntities = PopulateAssignedEntitiesToFilmProductionData(
                  filmProduction
            );

            var filmProductionCreationDetails = new CreateFilmProductionBindingModel
            {
                Title = filmProduction.Title,
                Duration = null,
                ReleaseDate = null,
                PlotSummary = filmProduction.PlotSummary,
                CountryId = filmProduction.CountryId,
                LanguageId = filmProduction.LanguageId,
                AssignedGenres = filmProductionRelatedEntities.Item1,
                AssignedActors = filmProductionRelatedEntities.Item2,
                AssignedDirectors = filmProductionRelatedEntities.Item3,
                AssignedScreenwriters = filmProductionRelatedEntities.Item4
            };

            return filmProductionCreationDetails;
        }

        public void CreateFilmProduction(
            CreateFilmProductionBindingModel createFilmProductionBindingModel,
            string[] selectedGenres,
            string[] selectedActors,
            string[] selectedDirectors,
            string[] selectedScreenwriters
        )
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(
                createFilmProductionBindingModel.ImageFile.FileName
            );
            string extension = Path.GetExtension(createFilmProductionBindingModel.ImageFile.FileName);
            string filmProductionImageName = fileName = fileName + DateTime.Now.ToString("yymmssffff")
                    + extension;
            string path = Path.Combine(wwwRootPath + "/images/film-productions", fileName);

            FilmProduction filmProductionToCreate = new FilmProduction
            {
                Title = createFilmProductionBindingModel.Title,
                Duration = (int)createFilmProductionBindingModel.Duration,
                ReleaseDate = (DateTime)createFilmProductionBindingModel.ReleaseDate,
                PlotSummary = createFilmProductionBindingModel.PlotSummary,
                CountryId = createFilmProductionBindingModel.CountryId,
                LanguageId = createFilmProductionBindingModel.LanguageId,
                ImageName = filmProductionImageName,
                ImageFile = createFilmProductionBindingModel.ImageFile
            };

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                filmProductionToCreate.ImageFile.CopyTo(fileStream);
            }

            if (selectedGenres != null)
            {
                foreach (var genreId in selectedGenres)
                {
                    var filmProductionGenreToAdd = new FilmProductionGenre
                    {
                        FilmProductionId = filmProductionToCreate.Id,
                        GenreId = genreId
                    };

                    filmProductionToCreate.FilmProductionGenres
                        .Add(filmProductionGenreToAdd);
                }
            }

            if (selectedActors != null)
            {
                foreach (var actorId in selectedActors)
                {
                    var filmProductionActorToAdd = new FilmProductionActor
                    {
                        FilmProductionId = filmProductionToCreate.Id,
                        ActorId = actorId
                    };

                    filmProductionToCreate.FilmProductionActors
                        .Add(filmProductionActorToAdd);
                }
            }

            if (selectedDirectors != null)
            {
                foreach (var directorId in selectedDirectors)
                {
                    var filmProductionDirectorToAdd = new FilmProductionDirector
                    {
                        FilmProductionId = filmProductionToCreate.Id,
                        DirectorId = directorId
                    };

                    filmProductionToCreate.FilmProductionDirectors
                        .Add(filmProductionDirectorToAdd);
                }
            }

            if (selectedScreenwriters != null)
            {
                foreach (var screenwriterId in selectedScreenwriters)
                {
                    var filmProductionScreenwriterToAdd = new FilmProductionScreenwriter
                    {
                        FilmProductionId = filmProductionToCreate.Id,
                        ScreenwriterId = screenwriterId
                    };

                    filmProductionToCreate.FilmProductionScreenwriters
                        .Add(filmProductionScreenwriterToAdd);
                }
            }

            _filmProductionRepository.Add(filmProductionToCreate);
        }

        public EditFilmProductionBindingModel GetFilmProductionEditingDetails(
            string filmProductionId
        )
        {
            var filmProductionToEdit = FindFilmProduction(filmProductionId);

            if (filmProductionToEdit is null)
            {
                return null;
            }

            var filmProductionRelatedEntities = PopulateAssignedEntitiesToFilmProductionData(
                filmProductionToEdit
            );

            var filmProductionToEditDetails = new EditFilmProductionBindingModel
            {
                Id = filmProductionToEdit.Id,
                Title = filmProductionToEdit.Title,
                Duration = filmProductionToEdit.Duration,
                ReleaseDate = filmProductionToEdit.ReleaseDate,
                PlotSummary = filmProductionToEdit.PlotSummary,
                CountryId = filmProductionToEdit.CountryId,
                LanguageId = filmProductionToEdit.LanguageId,
                ImageName = filmProductionToEdit.ImageName,
                AssignedGenres = filmProductionRelatedEntities.Item1,
                AssignedActors = filmProductionRelatedEntities.Item2,
                AssignedDirectors = filmProductionRelatedEntities.Item3,
                AssignedScreenwriters = filmProductionRelatedEntities.Item4
            };

            return filmProductionToEditDetails;
        }

        public void EditFilmProduction(
            EditFilmProductionBindingModel editFilmProductionBindingModel,
            string[] selectedGenres,
            string[] selectedActors,
            string[] selectedDirectors,
            string[] selectedScreenwriters
        )
        {
            var filmProductionToUpdate = _filmProductionRepository
                 .GetAllByCondition(fp => fp.Id == editFilmProductionBindingModel.Id)
                      .Include(fp => fp.FilmProductionGenres)
                           .ThenInclude(fpg => fpg.Genre)
                      .Include(fp => fp.FilmProductionActors)
                           .ThenInclude(fpa => fpa.Actor)
                      .Include(fp => fp.FilmProductionDirectors)
                            .ThenInclude(fpd => fpd.Director)
                      .Include(fp => fp.FilmProductionScreenwriters)
                            .ThenInclude(fps => fps.Screenwriter)
                      .FirstOrDefault();

            if (editFilmProductionBindingModel.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(
                        editFilmProductionBindingModel.ImageFile.FileName
                );
                string extension = Path.GetExtension(editFilmProductionBindingModel.ImageFile.FileName);
                string filmProductionImageName = fileName = fileName + DateTime.Now.ToString("yymmssffff")
                        + extension;

                string path = Path.Combine(wwwRootPath + "/images/film-productions", fileName);

                if (filmProductionToUpdate.ImageName != null)
                {
                    var existingImagePath = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images/film-productions",
                            filmProductionToUpdate.ImageName
                    );

                    if (File.Exists(existingImagePath))
                    {
                        File.Delete(existingImagePath);
                    }
                }

                filmProductionToUpdate.ImageName = filmProductionImageName;
                filmProductionToUpdate.ImageFile = editFilmProductionBindingModel.ImageFile;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    filmProductionToUpdate.ImageFile.CopyTo(fileStream);
                }
            }

            filmProductionToUpdate.Title = editFilmProductionBindingModel.Title;
            filmProductionToUpdate.Duration = editFilmProductionBindingModel.Duration;
            filmProductionToUpdate.Title = editFilmProductionBindingModel.Title;
            filmProductionToUpdate.Duration = editFilmProductionBindingModel.Duration;
            filmProductionToUpdate.ReleaseDate = editFilmProductionBindingModel.ReleaseDate;
            filmProductionToUpdate.PlotSummary = editFilmProductionBindingModel.PlotSummary;
            filmProductionToUpdate.CountryId = editFilmProductionBindingModel.CountryId;
            filmProductionToUpdate.LanguageId = editFilmProductionBindingModel.LanguageId;

            _filmProductionRepository.Update(filmProductionToUpdate);

            UpdateFilmProductionMappings(
                selectedGenres,
                selectedActors,
                selectedDirectors,
                selectedScreenwriters,
                filmProductionToUpdate
            );
        }

        public DeleteFilmProductionViewModel GetFilmProductionDeletionDetails(
            string filmProductionId
        )
        {
            var filmProductionToDelete = FindFilmProduction(filmProductionId);

            if (filmProductionToDelete is null)
            {
                return null;
            }

            var filmProductionToDeleteDetails = new DeleteFilmProductionViewModel
            {
                Title = filmProductionToDelete.Title,
                ReleaseDate = filmProductionToDelete.ReleaseDate,
                ImageName = filmProductionToDelete.ImageName
            };

            return filmProductionToDeleteDetails;
        }

        public void DeleteFilmProduction(FilmProduction filmProduction)
        {
            var filmProductionGenresByFilmProduction = _filmProductionGenreRepository
                    .GetAllByCondition(fpg => fpg.FilmProductionId == filmProduction.Id)
                        .ToArray();

            var filmProductionActorsByFilmProduction = _filmProductionActorRepository
                    .GetAllByCondition(fpa => fpa.FilmProductionId == filmProduction.Id)
                        .ToArray();

            var filmProductionDirectorsByFilmProduction = _filmProductonDirectorRepository
                    .GetAllByCondition(fpd => fpd.FilmProductionId == filmProduction.Id)
                        .ToArray();

            var filmProductionScreewritersByFilmProduction = _filmProductionScreenwriterRepository
                    .GetAllByCondition(fps => fps.FilmProductionId == filmProduction.Id)
                        .ToArray();

            _filmProductionGenreRepository.DeleteRange(filmProductionGenresByFilmProduction);
            _filmProductionActorRepository.DeleteRange(filmProductionActorsByFilmProduction);
            _filmProductonDirectorRepository.DeleteRange(filmProductionDirectorsByFilmProduction);
            _filmProductionScreenwriterRepository.DeleteRange(filmProductionScreewritersByFilmProduction);

            if (filmProduction.ImageName != null)
            {
                var existingImagePath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "images/film-productions",
                        filmProduction.ImageName
                );

                if (File.Exists(existingImagePath))
                {
                    File.Delete(existingImagePath);
                }
            }

            _filmProductionRepository.Delete(filmProduction);
        }

        public FilmProduction FindFilmProduction(string filmProductionId)
        {
            return _filmProductionRepository.GetById(filmProductionId);
        }

        private Tuple<List<AssignedGenreDataViewModel>,
            List<AssignedActorDataViewModel>,
            List<AssignedDirectorDataViewModel>,
            List<AssignedScreenwriterDataViewModel>>
            PopulateAssignedEntitiesToFilmProductionData(
            FilmProduction filmProduction
        )
        {
            var allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .ToList();

            var assignedGenreDataViewModel = new List<AssignedGenreDataViewModel>();
            var assignedActorDataViewModel = new List<AssignedActorDataViewModel>();
            var assignedDirectorDataViewModel = new List<AssignedDirectorDataViewModel>();
            var assignedScreenwriterDataViewModel = new List<AssignedScreenwriterDataViewModel>();

            var genresOfAFilmProduction = new HashSet<string>(
                     filmProduction.FilmProductionGenres
                           .Select(fpg => fpg.Genre.Id));

            var allGenres = _genreRepository
                    .GetAllAsNoTracking()
                        .ToList();

            foreach (var genre in allGenres)
            {
                assignedGenreDataViewModel.Add(new AssignedGenreDataViewModel
                {
                    GenreId = genre.Id,
                    Name = genre.Name,
                    IsAssigned = genresOfAFilmProduction.Contains(genre.Id)
                });
            }

            var actorsOfAFilmProduction = new HashSet<string>(
                     filmProduction.FilmProductionActors
                          .Select(fpa => fpa.Actor.Id));

            var allActors = _actorRepository
                    .GetAllAsNoTracking()
                        .ToList();

            foreach (var actor in allActors)
            {
                assignedActorDataViewModel.Add(new AssignedActorDataViewModel
                {
                    ActorId = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName,
                    IsAssigned = actorsOfAFilmProduction.Contains(actor.Id)
                });
            }

            var directorsOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionDirectors
                        .Select(fpd => fpd.Director.Id));

            var allDirectors = _directorRepository
                    .GetAllAsNoTracking()
                        .ToList();

            foreach (var director in allDirectors)
            {
                assignedDirectorDataViewModel.Add(new AssignedDirectorDataViewModel
                {
                    DirectorId = director.Id,
                    FirstName = director.FirstName,
                    LastName = director.LastName,
                    IsAssigned = directorsOfAFilmProduction.Contains(director.Id)
                });
            }

            var screenwritersOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionScreenwriters
                        .Select(fps => fps.Screenwriter.Id));

            var allScreenwriters = _screenwriterRepository
                    .GetAllAsNoTracking()
                        .ToList();

            foreach (var screenwriter in allScreenwriters)
            {
                assignedScreenwriterDataViewModel.Add(new AssignedScreenwriterDataViewModel
                {
                    ScreenwriterId = screenwriter.Id,
                    FirstName = screenwriter.FirstName,
                    LastName = screenwriter.LastName,
                    IsAssigned = screenwritersOfAFilmProduction.Contains(screenwriter.Id)
                });
            }

            return Tuple.Create(
                assignedGenreDataViewModel,
                assignedActorDataViewModel,
                assignedDirectorDataViewModel,
                assignedScreenwriterDataViewModel
            );
        }

        private void UpdateFilmProductionMappings(
           string[] selectedGenres,
           string[] selectedActors,
           string[] selectedDirectors,
           string[] selectedScreenwriters,
           FilmProduction filmProduction
        )
        {
            if (selectedGenres == null)
            {
                filmProduction.FilmProductionGenres = new List<FilmProductionGenre>();
                return;
            }

            var selectedGenresIds = new HashSet<string>(selectedGenres);

            var genresOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionGenres.Select(fpg => fpg.Genre.Id)
                );

            var allGenres = _genreRepository.GetAllAsNoTracking();

            foreach (var genre in allGenres)
            {
                if (selectedGenresIds.Contains(genre.Id))
                {
                    if (!genresOfAFilmProduction.Contains(genre.Id))
                    {
                        filmProduction.FilmProductionGenres.Add(new FilmProductionGenre
                        {
                            FilmProductionId = filmProduction.Id,
                            GenreId = genre.Id
                        });
                    }
                }
                else
                {
                    if (genresOfAFilmProduction.Contains(genre.Id))
                    {
                        FilmProductionGenre filmProductionGenreToRemove =
                           filmProduction.FilmProductionGenres
                               .FirstOrDefault(fpg =>
                                     fpg.GenreId == genre.Id
                                  );
                        _filmProductionGenreRepository.Delete(filmProductionGenreToRemove);
                    }
                }
            }

            if (selectedActors == null)
            {
                filmProduction.FilmProductionActors = new List<FilmProductionActor>();
                return;
            }

            var selectedActorsIds = new HashSet<string>(selectedActors);

            var actorsOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionActors.Select(fa => fa.Actor.Id)
                );

            var allActors = _actorRepository.GetAllAsNoTracking();

            foreach (var actor in allActors)
            {
                if (selectedActorsIds.Contains(actor.Id))
                {
                    if (!actorsOfAFilmProduction.Contains(actor.Id))
                    {
                        filmProduction.FilmProductionActors.Add(new FilmProductionActor
                        {
                            FilmProductionId = filmProduction.Id,
                            ActorId = actor.Id
                        });
                    }
                }
                else
                {
                    if (actorsOfAFilmProduction.Contains(actor.Id))
                    {
                        FilmProductionActor filmProductionActorToRemove =
                            filmProduction.FilmProductionActors
                                    .FirstOrDefault(fp =>
                                        fp.ActorId == actor.Id
                                    );

                        _filmProductionActorRepository.Delete(filmProductionActorToRemove);
                    }
                }
            }

            if (selectedDirectors == null)
            {
                filmProduction.FilmProductionDirectors = new List<FilmProductionDirector>();
                return;
            }

            var selectedDirectorsIds = new HashSet<string>(selectedDirectors);

            var directorsOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionDirectors.Select(fa => fa.Director.Id)
                );

            var allDirectors = _directorRepository.GetAllAsNoTracking();

            foreach (var director in allDirectors)
            {
                if (selectedDirectorsIds.Contains(director.Id))
                {
                    if (!directorsOfAFilmProduction.Contains(director.Id))
                    {
                        filmProduction.FilmProductionDirectors.Add(new FilmProductionDirector
                        {
                            FilmProductionId = filmProduction.Id,
                            DirectorId = director.Id
                        });
                    }
                }
                else
                {
                    if (directorsOfAFilmProduction.Contains(director.Id))
                    {
                        FilmProductionDirector filmProductionDirectorToRemove =
                            filmProduction.FilmProductionDirectors
                                    .FirstOrDefault(fp =>
                                        fp.DirectorId == director.Id
                                    );

                        _filmProductonDirectorRepository.Delete(filmProductionDirectorToRemove);
                    }
                }
            }

            if (selectedScreenwriters == null)
            {
                filmProduction.FilmProductionScreenwriters = new List<FilmProductionScreenwriter>();
                return;
            }

            var selectedScreenwritersIds = new HashSet<string>(selectedScreenwriters);

            var screenwritersOfAFilmProduction = new HashSet<string>(
                    filmProduction.FilmProductionScreenwriters.Select(fa => fa.Screenwriter.Id)
                );

            var allScreenwriters = _screenwriterRepository.GetAllAsNoTracking();

            foreach (var screenwriter in allScreenwriters)
            {
                if (selectedScreenwritersIds.Contains(screenwriter.Id))
                {
                    if (!screenwritersOfAFilmProduction.Contains(screenwriter.Id))
                    {
                        filmProduction.FilmProductionScreenwriters.Add(new FilmProductionScreenwriter
                        {
                            FilmProductionId = filmProduction.Id,
                            ScreenwriterId = screenwriter.Id
                        });
                    }
                }
                else
                {
                    if (screenwritersOfAFilmProduction.Contains(screenwriter.Id))
                    {
                        FilmProductionScreenwriter filmProductionScreenwriterToRemove =
                            filmProduction.FilmProductionScreenwriters
                                    .FirstOrDefault(fp =>
                                        fp.ScreenwriterId == screenwriter.Id
                                    );

                        _filmProductionScreenwriterRepository
                                .Delete(filmProductionScreenwriterToRemove);
                    }
                }
            }
        }
    }
}
