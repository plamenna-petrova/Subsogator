using Data.DataModels.Entities;

namespace Data.DataAccess.Seeding
{
    internal static class FilmProductionActorSeeder
    { 
        internal static FilmProductionActor[] FilmProductionActorSeedingArray { get; private set; }
            = SeedFilmProductionActors();

        private static FilmProductionActor[] SeedFilmProductionActors()
        {
            var filmProductionActorsToSeed = new FilmProductionActor[]
            {
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[0].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[1].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[0].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[2].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[3].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[1].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[4].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[5].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[2].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[6].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[7].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[3].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[8].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[9].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[10].Id
                },
                new FilmProductionActor()
                {
                    FilmProductionId = FilmProductionSeeder.FilmProductionSeedingArray[4].Id,
                    ActorId = ActorSeeder.ActorSeedingArray[11].Id
                }
            };

            return filmProductionActorsToSeed;
        }
    }
}
