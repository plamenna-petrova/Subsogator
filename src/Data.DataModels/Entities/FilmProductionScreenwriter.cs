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
