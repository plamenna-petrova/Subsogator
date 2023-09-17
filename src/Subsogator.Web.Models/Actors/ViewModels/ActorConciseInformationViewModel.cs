﻿using Subsogator.Common.GlobalConstants;
using System.ComponentModel;

namespace Subsogator.Web.Models.Actors.ViewModels
{
    public class ActorConciseInformationViewModel
    {
        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }
    }
}
