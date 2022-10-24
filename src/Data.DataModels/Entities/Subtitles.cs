using Data.DataModels.Abstraction;
using Data.DataModels.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public class Subtitles: BaseEntity
    {
        public string Name { get; set; }

        public string FilmProductionId { get; set; }

        public virtual FilmProduction FilmProduction { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
