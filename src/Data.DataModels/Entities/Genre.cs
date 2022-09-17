using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Name { get; set; }

        public virtual ICollection<FilmProductionGenre> FilmProductionGenres { get; set; }
    }
}
