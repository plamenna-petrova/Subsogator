using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Subsogator.Web.Models.FilmProductions.ViewModels
{
    public class FilmProductionFullDetailsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public int Duration { get; set; }

        [DisplayName("Release Date")]
        public DateTime ReleaseDate { get; set; }

        [DisplayName("Plot Summary")]
        public string PlotSummary { get; set; }

        [DisplayName("Country")]
        public string CountryName { get; set; }

        [DisplayName("Language")]
        public string LanguageName { get; set; }

        [DisplayName("Image")]
        public string ImageName { get; set; }

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

        [DisplayName("Genres")]
        public List<string> RelatedGenres { get; set; }

        [DisplayName("Actors")]
        public List<Tuple<string, string>> RelatedActors { get; set; }

        [DisplayName("Directors")]
        public List<Tuple<string, string>> RelatedDirectors { get; set; }

        [DisplayName("Screenwriters")]
        public List<Tuple<string, string>> RelatedScreenwriters { get; set; }
    }
}
