using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Web.Models.FilmProductions.BindingModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Languages.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.FilmProductions
{
    public class FilmProductionService : IFilmProductionService
    {
        private readonly IFilmProductionRepository _filmProductionRepository;

        public FilmProductionService(IFilmProductionRepository filmProductionRepository)
        {
            _filmProductionRepository = filmProductionRepository;
        }

        public IEnumerable<AllFilmProductionsViewModel> GetAllFilmProductions()
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
                LanguageName = singleFilmProduction.Language.Name
            };

            return singleFilmProductionDetails;
        }

        public void CreateFilmProduction(
            CreateFilmProductionBindingModel createFilmProductionBindingModel
        )
        {
            FilmProduction filmProductionToCreate = new FilmProduction
            {
                Title = createFilmProductionBindingModel.Title,
                Duration = (int) createFilmProductionBindingModel.Duration,
                ReleaseDate = (DateTime) createFilmProductionBindingModel.ReleaseDate,
                PlotSummary = createFilmProductionBindingModel.PlotSummary,
                CountryId = createFilmProductionBindingModel.CountryId,
                LanguageId = createFilmProductionBindingModel.LanguageId
            };

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

            var filmProductionToEditDetails = new EditFilmProductionBindingModel
            {
                Id = filmProductionToEdit.Id,
                Title = filmProductionToEdit.Title,
                Duration = filmProductionToEdit.Duration,
                ReleaseDate = filmProductionToEdit.ReleaseDate,
                PlotSummary = filmProductionToEdit.PlotSummary,
                CountryId = filmProductionToEdit.CountryId,
                LanguageId = filmProductionToEdit.LanguageId
            };

            return filmProductionToEditDetails;
        }

        public void EditFilmProduction(EditFilmProductionBindingModel 
            editFilmProductionBindingModel
        )
        {
            var filmProductionToUpdate = FindFilmProduction(editFilmProductionBindingModel.Id);

            filmProductionToUpdate.Title = editFilmProductionBindingModel.Title;
            filmProductionToUpdate.Duration = editFilmProductionBindingModel.Duration;
            filmProductionToUpdate.Title = editFilmProductionBindingModel.Title;
            filmProductionToUpdate.Duration = editFilmProductionBindingModel.Duration;
            filmProductionToUpdate.ReleaseDate = editFilmProductionBindingModel.ReleaseDate;
            filmProductionToUpdate.PlotSummary = editFilmProductionBindingModel.PlotSummary;
            filmProductionToUpdate.CountryId = editFilmProductionBindingModel.CountryId;
            filmProductionToUpdate.LanguageId = editFilmProductionBindingModel.LanguageId;

            _filmProductionRepository.Update(filmProductionToUpdate);
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
                ReleaseDate = filmProductionToDelete.ReleaseDate
            };

            return filmProductionToDeleteDetails;
        }

        public void DeleteFilmProduction(FilmProduction filmProduction)
        {
            _filmProductionRepository.Delete(filmProduction);
        }

        public FilmProduction FindFilmProduction(string filmProductionId)
        {
            return _filmProductionRepository.GetById(filmProductionId);
        }
    }
}
