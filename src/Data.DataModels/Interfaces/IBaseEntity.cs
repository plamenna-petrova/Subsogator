using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Interfaces
{
    public interface IBaseEntity<TKey>
    {
        [Key]
        TKey Id { get; set; }
    }
}
