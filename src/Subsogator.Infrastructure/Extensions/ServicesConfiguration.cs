using Data.DataAccess.Repositories;
using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Subsogator.Business.Services.Actors;
using Subsogator.Business.Services.Countries;
using Subsogator.Business.Services.Directors;
using Subsogator.Business.Services.Genres;
using Subsogator.Business.Transactions.Implementation;
using Subsogator.Business.Transactions.Interfaces;

namespace Subsogator.Infrastructure.Extensions
{
    public static class ServicesConfiguration
    {
        public static void RegisterServiceLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();

            serviceCollection.AddTransient<IActorService, ActorService>();
            serviceCollection.AddTransient<ICountryService, CountryService>();
            serviceCollection.AddTransient<IDirectorService, DirectorService>();
            serviceCollection.AddTransient<IGenreService, GenreService>();
        }
    }
}
