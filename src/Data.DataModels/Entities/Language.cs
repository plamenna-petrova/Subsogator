using Data.DataModels.Abstraction;
using Data.DataModels.Entities.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Entities
{
    public class Language: BaseEntity
    {
        public Language()
        {
            FilmProductions = new HashSet<FilmProduction>();
        }

        [Required]
        [MinLength(4)]
        [MaxLength(18)]
        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public virtual ICollection<FilmProduction> FilmProductions { get; set; }
    }
}
