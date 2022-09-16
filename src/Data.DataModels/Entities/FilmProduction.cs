using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public class FilmProduction: BaseEntity
    {
        public FilmProduction()
        {
            FilmProductionGenres = new HashSet<FilmProductionGenre>();
            FilmProductionActors = new HashSet<FilmProductionActor>();
            FilmProductionDirectors = new HashSet<FilmProductionDirector>();
            FilmProductionScreenwriters = new HashSet<FilmProductionScreenwriter>();
        }

        public string Title { get; set; }

        public int Duration { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string PlotSummary { get; set; }

        public virtual Subtitles Subtitles { get; set; }

        public string CountryId { get; set; }

        public virtual Country Country { get; set; }

        public string LanguageId { get; set; }

        public virtual Language Language { get; set; }

        public virtual ICollection<FilmProductionGenre> FilmProductionGenres { get; set; }

        public virtual ICollection<FilmProductionActor> FilmProductionActors { get; set; }

        public virtual ICollection<FilmProductionDirector> FilmProductionDirectors { get; set; }

        public virtual ICollection<FilmProductionScreenwriter> FilmProductionScreenwriters { get; set; }
    }
}
