using Subsogator.Common.GlobalConstants;
using Subsogator.Web.Models.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Actors.BindingModels
{
    public class CreateActorBindingModel
    {
        [Required]
        [StringLength(25, MinimumLength = 2,
            ErrorMessage = ValidationConstants.ActorFirstNameMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2,
            ErrorMessage = ValidationConstants.ActorLastNameMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }

        public IEnumerable<AssignedFilmProductionDataViewModel> AssignedFilmProductions { get; set; }
    }
}
