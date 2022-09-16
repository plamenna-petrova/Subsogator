using Data.DataModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess.Seeding
{
    internal static class ScreenwriterSeeder
    {
        internal static Screenwriter[] ScreenwriterSeedingArray { get; private set; }
            = SeedScreenwriters();

        private static Screenwriter[] SeedScreenwriters()
        {
            var screenwritersToSeed = new Screenwriter[]
            {
               new Screenwriter()
               {
                   FirstName = "Christopher",
                   LastName = "Nolan",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Robert",
                   LastName = "Ludlum",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Tony",
                   LastName = "Gilroy",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Lilly",
                   LastName = "Wachowski",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Lana",
                   LastName = "Wachowski",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Bob",
                   LastName = "Kane",
                   CreatedOn = DateTime.UtcNow
               },
               new Screenwriter()
               {
                   FirstName = "Jonathan",
                   LastName = "Nolan",
                   CreatedOn = DateTime.UtcNow
               }
            };

            return screenwritersToSeed;
        }
    }
}
