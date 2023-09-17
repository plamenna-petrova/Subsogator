using System.Collections.Generic;

namespace Data.DataModels.Entities
{
    public class Director: CrewMember
    {
        public Director()
        {
            FilmProductionDirectors = new HashSet<FilmProductionDirector>();
        }

        public virtual ICollection<FilmProductionDirector> FilmProductionDirectors { get; set; }
    }
}
