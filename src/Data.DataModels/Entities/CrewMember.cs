using Data.DataModels.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Entities
{
    public abstract class CrewMember: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(25)]
        public string LastName { get; set; }
    }
}
