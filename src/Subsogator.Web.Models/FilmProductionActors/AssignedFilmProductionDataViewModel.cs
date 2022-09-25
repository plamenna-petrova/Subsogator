using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.FilmProductionActors
{
    public class AssignedFilmProductionDataViewModel
    {
        public string FilmProductionId { get; set; }

        public string Title { get; set; }

        public bool IsAssigned { get; set; }
    }
}
