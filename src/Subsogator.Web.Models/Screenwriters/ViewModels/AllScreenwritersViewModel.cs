using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Subsogator.Web.Models.Screenwriters.ViewModels
{
    public class AllScreenwritersViewModel
    {
        public string Id { get; set; }

        [DisplayName(DisplayConstants.FirstNameDisplayName)]
        public string FirstName { get; set; }

        [DisplayName(DisplayConstants.LastNameDisplayName)]
        public string LastName { get; set; }
    }
}
