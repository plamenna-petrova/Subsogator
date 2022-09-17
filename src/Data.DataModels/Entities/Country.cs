using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public class Country: BaseEntity
    {
        public Country()
        {
            FilmProductions = new HashSet<FilmProduction>();
        }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        public virtual ICollection<FilmProduction> FilmProductions { get; set; }
    }
}
