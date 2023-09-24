using Data.DataModels.Entities;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Screenwriters
{
    public interface IScreenwriterService
    {
        IEnumerable<AllScreenwritersViewModel> GetAllScreenwriters();

        CreateScreenwriterBindingModel GetScreenwriterCreatingDetails();

        ScreenwriterDetailsViewModel GetScreenwriterDetails(string directorId);

        bool CreateScreenwriter(CreateScreenwriterBindingModel createScreenwriterBindingModel, string[] selectedFilmProductions, string currentUserName);

        EditScreenwriterBindingModel GetScreenwriterEditingDetails(string directorId);

        bool EditScreenwriter(EditScreenwriterBindingModel editScreenwriterBindingModel, string[] selectedFilmProductions, string currentUserName);

        DeleteScreenwriterViewModel GetScreenwriterDeletionDetails(string directorId);

        void DeleteScreenwriter(Screenwriter director);

        Screenwriter FindScreenwriter(string directorId);
    }
}
