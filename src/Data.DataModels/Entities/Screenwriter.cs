using System.Collections.Generic;

namespace Data.DataModels.Entities
{
    public class Screenwriter: CrewMember
    {
        public Screenwriter()
        {
            FilmProductionScreenwriters = new HashSet<FilmProductionScreenwriter>();
        }

        public virtual ICollection<FilmProductionScreenwriter> FilmProductionScreenwriters { get; set; }
    }
}
