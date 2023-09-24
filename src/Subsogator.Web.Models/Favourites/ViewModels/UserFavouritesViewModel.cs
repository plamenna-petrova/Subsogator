using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Favourites.ViewModels
{
    public class UserFavouritesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string UploaderUserName { get; set; }

        public FilmProductionConciseInformationViewModel RelatedFilmProduction { get; set; }
    }
}
