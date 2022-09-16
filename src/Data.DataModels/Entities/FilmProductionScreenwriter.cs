using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public class FilmProductionScreenwriter
    {
        public string FilmProductionId { get; set; }

        public virtual FilmProduction FilmProduction { get; set; }

        public string ScreenwriterId { get; set; }

        public virtual Screenwriter Screenwriter { get; set; }
    }
}
