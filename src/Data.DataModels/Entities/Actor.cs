using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
