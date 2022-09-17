using Subsogator.Common.GlobalConstants;
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
        [MinLength(2, 
            ErrorMessage = ValidationConstants.ActorFirstNameMinimumLengthValidationMessage)]
        [MaxLength(25, 
            ErrorMessage = ValidationConstants.ActorFirstNameMaximumLengthValidationMessage)]
        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, 
            ErrorMessage = ValidationConstants.ActorLastNameMinimumLengthValidationMessage)]
        [MaxLength(25, 
            ErrorMessage = ValidationConstants.ActorLastNameMaximumLengthValidationMessage)]
        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }
    }
}
