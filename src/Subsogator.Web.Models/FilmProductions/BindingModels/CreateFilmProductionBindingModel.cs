using Microsoft.AspNetCore.Http;
using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Models.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.FilmProductions.BindingModels
{
    public class CreateFilmProductionBindingModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2,
            ErrorMessage = ValidationConstants.FilmProductionTitleMinimumLengthValidationMessage)]
        public string Title { get; set; }

        [Required]
        [Range(45, 240, ErrorMessage = ValidationConstants.FilmProductionDurationRangeValidationMessage)]
        public int? Duration { get; set; } = null;

        [Required]
        [DisplayName(DisplayConstants.FilmProductionReleaseDateDisplayName)]
        public DateTime? ReleaseDate { get; set; } = null;

        [Required]
        [StringLength(500, MinimumLength = 2, 
                ErrorMessage = ValidationConstants.FilmProductionPlotSummaryMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.FilmProductionPlotSummaryDisplayName)]
        public string PlotSummary { get; set; }

        public string CountryId { get; set; }

        public string LanguageId { get; set; }

        [DisplayName("Upload Image")]
        public IFormFile? ImageFile { get; set; }

        public IEnumerable<AssignedGenreDataViewModel> AssignedGenres { get; set; }

        public IEnumerable<AssignedActorDataViewModel> AssignedActors { get; set; }

        public IEnumerable<AssignedDirectorDataViewModel> AssignedDirectors { get; set; }

        public IEnumerable<AssignedScreenwriterDataViewModel> AssignedScreenwriters { get; set; }
    }
}
