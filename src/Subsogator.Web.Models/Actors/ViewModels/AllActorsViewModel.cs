﻿using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Actors.ViewModels
{
    public class AllActorsViewModel
    {
        public string Id { get; set; }

        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
