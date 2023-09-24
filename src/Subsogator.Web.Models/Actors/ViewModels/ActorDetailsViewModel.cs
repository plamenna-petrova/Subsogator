using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Models.Actors.ViewModels
{
    public class ActorDetailsViewModel
    {
        public string Id { get; set; }

        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }

        [DisplayName(DisplayConstants.CreatedOnDisplayName)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(DisplayConstants.CreatedByDisplayName)]
        public string CreatedBy { get; set; }

        [DisplayName(DisplayConstants.ModifiedOnDisplayName)]
        [DisplayFormat(NullDisplayText = DisplayConstants.NullModifiedOnEntryDisplayName)]
        public DateTime? ModifiedOn { get; set; }

        [DisplayName(DisplayConstants.ModifiedByDisplayName)]
        [DisplayFormat(NullDisplayText = DisplayConstants.NullModifiedByEntryDisplayName)]
        public string ModifiedBy { get; set; }

        public IEnumerable<FilmProductionDetailedInformationViewModel> RelatedFilmProductions { get; set; }
    }
}
