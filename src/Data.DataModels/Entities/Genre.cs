using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public class Genre: BaseEntity
    {
        public Genre()
        {
            FilmProductionGenres = new HashSet<FilmProductionGenre>();
        }

        public string Name { get; set; }

        public virtual ICollection<FilmProductionGenre> FilmProductionGenres { get; set; }
    }
}
