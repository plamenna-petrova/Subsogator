using Subsogator.Web.Models.Comments.BindingModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Comments
{
    public interface ICommentService
    {
        bool CreateComment(CreateCommentBindingModel createCommentBindingModel, string subtitlesId, string userId);

        IEnumerable<AllCommentsForSubtitlesCatalogueItemViewModel> GetAllCommentsForSubtitlesCatalogueItem(string subtitlesId);
    }
}
