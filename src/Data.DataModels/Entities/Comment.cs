using Data.DataModels.Abstraction;
using Data.DataModels.Entities.Identity;

namespace Data.DataModels.Entities
{
    public class Comment: BaseEntity
    {
        public string Content { get; set; }

        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SubtitlesId { get; set; }

        public virtual Subtitles Subtitles { get; set; }
    }
}
