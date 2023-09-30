using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.AspNetCore.Hosting;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Subtitles.BindingModels;
using Subsogator.Web.Models.Subtitles.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Subsogator.Business.Services.Subtitles
{
    public class SubtitlesService : ISubtitlesService
    {
        private readonly ISubtitlesRepository _subtitlesRepository;

        private readonly ISubtitlesFilesRepository _subtitlesFilesRepository;

        private readonly IFilmProductionRepository _filmProductionRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public SubtitlesService(
            ISubtitlesRepository subtitlesRepository,
            ISubtitlesFilesRepository subtitlesFilesRepository,
            IFilmProductionRepository filmProductionRepository,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _subtitlesRepository = subtitlesRepository;
            _subtitlesFilesRepository = subtitlesFilesRepository;
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
                .Find(fp => fp.Id == createSubtitlesBindingModel.FilmProductionId);

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

            if (createSubtitlesBindingModel.Files is null || createSubtitlesBindingModel.Files.Count() == 0)
            {
                return false;
            }

            _subtitlesRepository.Add(subtitlesToCreate);

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string directoryName = Path
                .GetDirectoryName(@$"{wwwRootPath}\archives\subtitles\{createSubtitlesBindingModel.FilmProductionId}\");

            Directory.CreateDirectory(directoryName);

            foreach (var file in createSubtitlesBindingModel.Files)
            {
                string outputPath = Path
                  .Combine(wwwRootPath + @$"\archives\subtitles\{createSubtitlesBindingModel.FilmProductionId}");

                string path = Path.Combine(outputPath, file.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var subtitlesFiles = new SubtitlesFiles
                {
                    FileName = file.FileName,
                    SubtitlesId = subtitlesToCreate.Id
                };

                _subtitlesFilesRepository.Add(subtitlesFiles);
            }

            return true;
        }

        public EditSubtitlesBindingModel GetSubtitlesEditingDetails(string actorId)
        {
            var subtitlesToEdit = _subtitlesRepository
                .GetAllByCondition(s => s.Id == actorId)
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
                    .GetAllByCondition(s => s.Id == editSubtitlesBindingModel.Id)
                          .FirstOrDefault();

            var allFilmProductions = _filmProductionRepository.GetAllAsNoTracking().ToList();
            var relatedFilmProduction = allFilmProductions
                  .Find(fp => fp.Id == editSubtitlesBindingModel.FilmProductionId) ?? subtitlesToUpdate.FilmProduction;

            subtitlesToUpdate.FilmProductionId = relatedFilmProduction.Id;
            subtitlesToUpdate.Name = $"{relatedFilmProduction.Title} {relatedFilmProduction.ReleaseDate.Year}";
            subtitlesToUpdate.ApplicationUserId = userId;

            var filteredSubtitles = _subtitlesRepository
                .GetAllAsNoTracking()
                    .Where(s => !s.Id.Equals(subtitlesToUpdate.Id))
                      .AsQueryable();

            if (_subtitlesRepository.Exists(filteredSubtitles, subtitlesToUpdate))
            {
                return false;
            }

            _subtitlesRepository.Update(subtitlesToUpdate);

            if (editSubtitlesBindingModel.Files != null && editSubtitlesBindingModel.Files.Count() > 0)
            {
                var existingSubtitlesFiles = GetSubtitlesFilesBySubtitlesId(subtitlesToUpdate.Id);

                if (existingSubtitlesFiles != null)
                {
                    _subtitlesFilesRepository.DeleteRange(existingSubtitlesFiles.ToArray());
                }

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string subtitlesForFilmProductionDirectoryName = Path
                    .GetDirectoryName(@$"{wwwRootPath}\archives\subtitles\{relatedFilmProduction.Id}\");

                if (!Directory.Exists(subtitlesForFilmProductionDirectoryName))
                {
                    Directory.CreateDirectory(subtitlesForFilmProductionDirectoryName);
                }
                else
                {
                    DirectoryInfo subtitlesForFilmProductionDirectoryInfo = 
                        new DirectoryInfo(subtitlesForFilmProductionDirectoryName);

                    foreach (FileInfo subtitlesFileInfo in subtitlesForFilmProductionDirectoryInfo.GetFiles())
                    {
                        subtitlesFileInfo.Delete();
                    }
                }

                foreach (var file in editSubtitlesBindingModel.Files)
                {
                    string outputPath = Path
                      .Combine(wwwRootPath + @$"\archives\subtitles\{relatedFilmProduction.Id}");

                    string path = Path.Combine(outputPath, file.FileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var subtitlesFiles = new SubtitlesFiles
                    {
                        FileName = file.FileName,
                        SubtitlesId = subtitlesToUpdate.Id
                    };

                    _subtitlesFilesRepository.Add(subtitlesFiles);
                }
            }

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
            var subtitlesFilesToDelete = _subtitlesFilesRepository
                  .GetAllByCondition(sf => sf.SubtitlesId == sf.SubtitlesId)
                      .ToArray();

            _subtitlesFilesRepository.DeleteRange(subtitlesFilesToDelete);

            _subtitlesRepository.Delete(subtitles);
        }

        public List<SubtitlesFiles> GetSubtitlesFilesBySubtitlesId(string id)
        {
            return _subtitlesFilesRepository
                .GetAllAsNoTracking()
                .Where(sf => sf.SubtitlesId == id)
                .ToList();
        }

        public Data.DataModels.Entities.Subtitles FindSubtitles(string subtitlesId)
        {
            return _subtitlesRepository.GetById(subtitlesId);
        }
    }
}
