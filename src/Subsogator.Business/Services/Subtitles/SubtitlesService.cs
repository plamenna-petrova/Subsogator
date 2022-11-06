using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Subsogator.Web.Models.Actors.BindingModels;
using Subsogator.Web.Models.Actors.ViewModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Subtitles.BindingModels;
using Subsogator.Web.Models.Subtitles.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subsogator.Business.Services.Subtitles
{
    public class SubtitlesService : ISubtitlesService
    {
        private readonly ISubtitlesRepository _subtitlesRepository;

        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public SubtitlesService(
            ISubtitlesRepository subtitlesRepository,
            IFilmProductionRepository filmProductionRepository,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _subtitlesRepository = subtitlesRepository;
            _filmProductionRepository = filmProductionRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Data.DataModels.Entities.Subtitles> GetAllToList()
        {
            return _subtitlesRepository.GetAll().ToList();
        }

        public IEnumerable<AllSubtitlesViewModel> GetAllSubtitles()
        {
            List<AllSubtitlesViewModel> allSubtitles = _subtitlesRepository
                .GetAllAsNoTracking()
                    .Select(s => new AllSubtitlesViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        RelatedFilmProduction = new FilmProductionConciseInformationViewModel
                        {
                            Title = s.FilmProduction.Title
                        }
                    })
                    .ToList();

            return allSubtitles;
        }

        public SubtitlesDetailsViewModel GetSubtitlesDetails(string subtitlesId)
        {
            var singleSubtitles = _subtitlesRepository
                .GetAllByCondition(s => s.Id == subtitlesId)
                    .FirstOrDefault();

            if (singleSubtitles is null)
            {
                return null;
            }

            var singleSubtitlesDetails = new SubtitlesDetailsViewModel
            {
                Id = singleSubtitles.Id,
                Name = singleSubtitles.Name,
                CreatedOn = singleSubtitles.CreatedOn,
                ModifiedOn = singleSubtitles.ModifiedOn,
                RelatedFilmProduction = new FilmProductionDetailedInformationViewModel
                {
                    Title = singleSubtitles.FilmProduction.Title,
                    Duration = singleSubtitles.FilmProduction.Duration,
                    ReleaseDate = singleSubtitles.FilmProduction.ReleaseDate
                }
            };

            return singleSubtitlesDetails;
        }

        public bool CreateSubtitles(CreateSubtitlesBindingModel createSubtitlesBindingModel, string userId)
        {
            var allFilmProductions = _filmProductionRepository
                .GetAllAsNoTracking()
                    .ToList();

            var relatedFilmProduction = allFilmProductions
                .Find(f => f.Id == createSubtitlesBindingModel.FilmProductionId);

            if (createSubtitlesBindingModel.Files is null || createSubtitlesBindingModel.Files.Count() == 0)
            {
                return false;
            }

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string directoryName = Path.GetDirectoryName(@$"{wwwRootPath}\archives\subtitles\{createSubtitlesBindingModel.FilmProductionId}\");

            Directory.CreateDirectory(directoryName);

            foreach (var file in createSubtitlesBindingModel.Files)
            {
                string outputPath = Path.Combine(wwwRootPath + @$"\archives\subtitles\{createSubtitlesBindingModel.FilmProductionId}");

                string path = Path.Combine(outputPath, file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            Data.DataModels.Entities.Subtitles subtitlesToCreate = new Data.DataModels.Entities.Subtitles
            {
                FilmProductionId = createSubtitlesBindingModel.FilmProductionId,
                Name = $"{relatedFilmProduction.Title} {relatedFilmProduction.ReleaseDate.Year}",
                ApplicationUserId = userId
            };

            var allSubtitles = _subtitlesRepository.GetAllAsNoTracking();

            if (_subtitlesRepository.Exists(allSubtitles, subtitlesToCreate))
            {
                return false;
            }

            _subtitlesRepository.Add(subtitlesToCreate);

            return true;
        }

        public EditSubtitlesBindingModel GetSubtitlesEditingDetails(string actorId)
        {
            var subtitlesToEdit = _subtitlesRepository
                    .GetAllByCondition(a => a.Id == actorId)
                                .FirstOrDefault();

            if (subtitlesToEdit is null)
            {
                return null;
            }

            var subtitlesToEditDetails = new EditSubtitlesBindingModel
            {
                Id = subtitlesToEdit.Id,
                Name = subtitlesToEdit.Name,
                FilmProductionId = subtitlesToEdit.FilmProductionId
            };

            return subtitlesToEditDetails;
        }

        public bool EditSubtitles(EditSubtitlesBindingModel editSubtitlesBindingModel, string userId)
        {
            var subtitlesToUpdate = _subtitlesRepository
                    .GetAllByCondition(a => a.Id == editSubtitlesBindingModel.Id)
                          .FirstOrDefault();

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking().ToList();
            var relatedFilmProduction = allFilmProductions
                  .Find(f => f.Id == editSubtitlesBindingModel.FilmProductionId);

            subtitlesToUpdate.FilmProductionId = editSubtitlesBindingModel.FilmProductionId;
            subtitlesToUpdate.Name = $"{relatedFilmProduction.Title} {relatedFilmProduction.ReleaseDate.Year}";
            subtitlesToUpdate.ApplicationUserId = userId;

            var filteredSubtitles = _subtitlesRepository
            .GetAllAsNoTracking()
                    .Where(a => !a.Id.Equals(subtitlesToUpdate.Id))
                        .AsQueryable();

            if (_subtitlesRepository.Exists(filteredSubtitles, subtitlesToUpdate))
            {
                return false;
            }

            _subtitlesRepository.Update(subtitlesToUpdate);

            return true;
        }

        public DeleteSubtitlesViewModel GetSubtitlesDeletionDetails(string subtitlesId)
        {
            var subtitlesToDelete = FindSubtitles(subtitlesId);

            if (subtitlesToDelete is null)
            {
                return null;
            }

            var subtitlesToDeleteDetails = new DeleteSubtitlesViewModel
            {
                Name = subtitlesToDelete.Name
            };

            return subtitlesToDeleteDetails;
        }

        public void DeleteSubtitles(Data.DataModels.Entities.Subtitles subtitles)
        {
            _subtitlesRepository.Delete(subtitles);
        }

        public Data.DataModels.Entities.Subtitles FindSubtitles(string subtitlesId)
        {
            return _subtitlesRepository.GetById(subtitlesId);
        }
    }
}
