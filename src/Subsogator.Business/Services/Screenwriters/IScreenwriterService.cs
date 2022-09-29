using Data.DataModels.Entities;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Screenwriters
{
    public interface IScreenwriterService
    {
        IEnumerable<AllScreenwritersViewModel> GetAllScreenwriters();

        CreateScreenwriterBindingModel GetScreenwriterCreatingDetails();

        ScreenwriterDetailsViewModel GetScreenwriterDetails(string directorId);

        bool CreateScreenwriter(CreateScreenwriterBindingModel createScreenwriterBindingModel, string[] selectedFilmProductions);

        EditScreenwriterBindingModel GetScreenwriterEditingDetails(string directorId);

        bool EditScreenwriter(EditScreenwriterBindingModel editScreenwriterBindingModel, string[] selectedFilmProductions);

        DeleteScreenwriterViewModel GetScreenwriterDeletionDetails(string directorId);

        void DeleteScreenwriter(Screenwriter director);

        Screenwriter FindScreenwriter(string directorId);
    }
}
