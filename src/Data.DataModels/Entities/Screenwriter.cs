using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
