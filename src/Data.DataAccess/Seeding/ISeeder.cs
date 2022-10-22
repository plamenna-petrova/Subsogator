using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    public interface ISeeder
    {
        Task<bool> SeedDatabase(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider);
    }
}
