using Data.DataModels.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Entities
{
    public abstract class CrewMember: BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
