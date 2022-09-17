using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Subsogator.Web.Models.Countries.ViewModels
{
    public class AllCountriesViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [DisplayName(DisplayConstants.CreatedOnDisplayName)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(DisplayConstants.ModifiedOnDisplayName)]
        [DisplayFormat(NullDisplayText = "Not Yet Modified")]
        public DateTime? ModifiedOn { get; set; }
    }
}
