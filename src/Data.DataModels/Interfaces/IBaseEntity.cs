using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataModels.Interfaces
{
    public interface IBaseEntity<TKey>
    {
        [Key]
        TKey Id { get; set; }
    }
}
