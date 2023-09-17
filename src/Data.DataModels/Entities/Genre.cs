using Data.DataModels.Abstraction;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
