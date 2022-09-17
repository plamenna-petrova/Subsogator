﻿using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Screenwriters.BindingModels
{
    public class CreateScreenwriterBindingModel
    {
        [Required]
        [StringLength(25, MinimumLength = 2,
            ErrorMessage = ValidationConstants.ScreenwriterFirstNameMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 2,
            ErrorMessage = ValidationConstants.ScreenwriterLastNameMinimumLengthValidationMessage)]
        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }
    }
}