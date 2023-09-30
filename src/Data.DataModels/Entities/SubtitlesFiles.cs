using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataModels.Entities
{
    public class SubtitlesFiles : BaseEntity
    {
        public string FileName { get; set; }

        public string SubtitlesId { get; set; }
    }
}
