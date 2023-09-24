using Subsogator.Web.Models.Favourites.ViewModels;
using Subsogator.Web.Models.Subtitles.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Favourites
{
    public interface IFavouritesService
    {
        IEnumerable<UserFavouritesViewModel> GetAllUserFavourites(string userId);

        bool AddToFavourites(string userId, string subtitlesId);

        void RemoveFromFavourites(Data.DataModels.Entities.Favourites favourites);

        Data.DataModels.Entities.Favourites FindFavourites(string userId, string subtiltesId);
    }
}
