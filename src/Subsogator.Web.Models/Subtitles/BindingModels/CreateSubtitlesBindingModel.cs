using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Web.Models.Subtitles.BindingModels
{
    public class CreateSubtitlesBindingModel
    {
        public string Name { get; set; }

        public string FilmProductionId { get; set; }

        public List<IFormFile> Files { get; set; }
    }
}
