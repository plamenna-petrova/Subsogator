using Data.DataModels.Entities;
using Subsogator.Web.Models.Languages.BindingModels;
using Subsogator.Web.Models.Languages.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Languages
{
    public interface ILanguageService
    {
        IEnumerable<AllLanguagesViewModel> GetAllLanguages();

        LanguageDetailsViewModel GetLanguageDetails(string languageId);

        bool CreateLanguage(CreateLanguageBindingModel createLanguageBindingModel);

        EditLanguageBindingModel GetLanguageEditingDetails(string languageId);

        bool EditLanguage(EditLanguageBindingModel editLanguageBindingModel);

        DeleteLanguageViewModel GetLanguageDeletionDetails(string languageId);

        void DeleteLanguage(Language language);

        Language FindLanguage(string languageId);
    }
}
