using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Languages.BindingModels;
using Subsogator.Web.Models.Languages.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Subsogator.Business.Services.Languages
{
    public class LanguageService: ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public List<Language> GetAllLanguages()
        {
            return _languageRepository.GetAllAsNoTracking().ToList();
        }

        public IEnumerable<AllLanguagesViewModel> GetAllLanguagesWithRelatedData()
        {
            List<AllLanguagesViewModel> allLanguages = _languageRepository
                .GetAllAsNoTracking()
                .Select(l => new AllLanguagesViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    RelatedFilmProductions = l.FilmProductions
                        .Select(fp => new FilmProductionConciseInformationViewModel 
                        {
                            Title = fp.Title
                        })
                })
                .ToList();

            return allLanguages;
        }

        public LanguageDetailsViewModel GetLanguageDetails(string languageId)
        {
            var singleLanguage = _languageRepository
                  .GetAllByCondition(l => l.Id == languageId)
                    .Include(l => l.FilmProductions)
                       .FirstOrDefault();

            if (singleLanguage is null)
            {
                return null;
            }

            var singleLanguageDetails = new LanguageDetailsViewModel
            {
                Id = singleLanguage.Id,
                Name = singleLanguage.Name,
                CreatedOn = singleLanguage.CreatedOn,
                ModifiedOn = singleLanguage.ModifiedOn,
                RelatedFilmProductions = singleLanguage.FilmProductions
                    .Select(fp => new FilmProductionDetailedInformationViewModel 
                    {
                        Title = fp.Title,
                        Duration = fp.Duration,
                        ReleaseDate = fp.ReleaseDate
                    })
            };

            return singleLanguageDetails;
        }

        public bool CreateLanguage(CreateLanguageBindingModel createLanguageBindingModel)
        {
            Language languageToCreate = new Language
            {
                Name = createLanguageBindingModel.Name
            };

            var allLanguages = _languageRepository.GetAllAsNoTracking();

            if (_languageRepository.Exists(allLanguages, languageToCreate))
            {
                return false;
            }

            _languageRepository.Add(languageToCreate);

            return true;
        }

        public EditLanguageBindingModel GetLanguageEditingDetails(string languageId)
        {
            var languageToEdit = FindLanguage(languageId);

            if (languageToEdit is null)
            {
                return null;
            }

            var countyToEditDetails = new EditLanguageBindingModel
            {
                Id = languageToEdit.Id,
                Name = languageToEdit.Name
            };

            return countyToEditDetails;
        }

        public bool EditLanguage(EditLanguageBindingModel editLanguageBindingModel)
        {
            var languageToUpdate = FindLanguage(editLanguageBindingModel.Id);

            languageToUpdate.Name = editLanguageBindingModel.Name;

            var filteredLanguages = _languageRepository
                .GetAllAsNoTracking()
                .Where(l => !l.Id.Equals(languageToUpdate.Id));

            if (_languageRepository.Exists(filteredLanguages, languageToUpdate))
            {
                return false;
            }

            _languageRepository.Update(languageToUpdate);

            return true;
        }

        public DeleteLanguageViewModel GetLanguageDeletionDetails(string languageId)
        {
            var languageToDelete = FindLanguage(languageId);

            if (languageToDelete is null)
            {
                return null;
            }

            var languageToDeleteDetails = new DeleteLanguageViewModel
            {
                Name = languageToDelete.Name
            };

            return languageToDeleteDetails;
        }

        public void DeleteLanguage(Language language)
        {
            _languageRepository.Delete(language);
        }

        public Language FindLanguage(string languageId)
        {
            return _languageRepository.GetById(languageId);
        }
    }
}
