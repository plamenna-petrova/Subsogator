using Data.DataModels.Abstraction;
using Data.DataModels.Entities.Identity;
using System.Collections.Generic;

namespace Data.DataModels.Entities
{
    public class Subtitles: BaseEntity
    {
        public Subtitles()
        {
            Comments = new HashSet<Comment>();
            Favourites = new HashSet<Favourites>();
        }

        public string Name { get; set; }

        public string FilmProductionId { get; set; }

        public virtual FilmProduction FilmProduction { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Favourites> Favourites { get; set; }
    }
}
