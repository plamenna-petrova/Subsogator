using Data.DataModels.Entities.Identity;
using System.Collections.Generic;

namespace Data.DataModels.Entities
{
    public class Actor: CrewMember
    {
        public Actor()
        {
            FilmProductionActors = new HashSet<FilmProductionActor>();
        }

        public virtual ICollection<FilmProductionActor> FilmProductionActors { get; set; }
    }
}
