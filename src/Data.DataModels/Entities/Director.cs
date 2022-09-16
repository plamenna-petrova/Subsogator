using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
