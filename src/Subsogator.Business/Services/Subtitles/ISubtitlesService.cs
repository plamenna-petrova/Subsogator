using Subsogator.Web.Models.Subtitles.BindingModels;
using Subsogator.Web.Models.Subtitles.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Subtitles
{
    public interface ISubtitlesService
    {
        List<Data.DataModels.Entities.Subtitles> GetAllToList();

        IEnumerable<AllSubtitlesViewModel> GetAllSubtitles();

        SubtitlesDetailsViewModel GetSubtitlesDetails(string subtitlesId);

        bool CreateSubtitles(CreateSubtitlesBindingModel createSubtitlesBindingModel, string userId);

        EditSubtitlesBindingModel GetSubtitlesEditingDetails(string subtitlesId);

        bool EditSubtitles(EditSubtitlesBindingModel editSubtitlesBindingModel, string userId);

        DeleteSubtitlesViewModel GetSubtitlesDeletionDetails(string subtitlesId);

        void DeleteSubtitles(Data.DataModels.Entities.Subtitles subtitles);

        Data.DataModels.Entities.Subtitles FindSubtitles(string subtielsId);
    }
}
