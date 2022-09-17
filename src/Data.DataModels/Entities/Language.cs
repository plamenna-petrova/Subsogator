using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public virtual ICollection<FilmProduction> FilmProductions { get; set; }
    }
}
