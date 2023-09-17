using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Subsogator.Web.Models.Subtitles.BindingModels
{
    public class EditSubtitlesBindingModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string FilmProductionId { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
