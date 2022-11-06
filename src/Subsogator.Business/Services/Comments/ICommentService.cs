using Subsogator.Web.Models.Comments.BindingModels;
using Subsogator.Web.Models.Countries.BindingModels;
using Subsogator.Web.Models.Countries.ViewModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Comments
{
    public interface ICommentService
    {
        bool CreateComment(CreateCommentBindingModel createCommentBindingModel, string subtitlesId, string userId);

        IEnumerable<AllCommentsForSubtitlesCatalogueItemViewModel> GetAllCommentsForSubtitlesCatalogueItem(string subtitlesId);
    }
}
