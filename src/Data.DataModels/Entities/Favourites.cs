﻿using Data.DataModels.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataModels.Entities
{
    public class Favourites
    {
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public string SubtitlesId { get; set; }

        public virtual Subtitles Subtitles { get; set; }
    }
}
