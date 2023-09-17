using System;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    public interface ISeeder
    {
        Task<bool> SeedDatabase(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider);
    }
}
