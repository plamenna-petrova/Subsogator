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

        ScreenwriterDetailsViewModel GetScreenwriterDetails(string screenwriterId);

        bool CreateScreenwriter(CreateScreenwriterBindingModel createScreenwriterBindingModel);

        EditScreenwriterBindingModel GetScreenwriterEditingDetails(string screenwriterId);

        bool EditScreenwriter(EditScreenwriterBindingModel editScreenwriterBindingModel);

        DeleteScreenwriterViewModel GetScreenwriterDeletionDetails(string screenwriterId);

        void DeleteScreenwriter(Screenwriter screenwriter);

        Screenwriter FindScreenwriter(string screenwriterId);
    }
}
