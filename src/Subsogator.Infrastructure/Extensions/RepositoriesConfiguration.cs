using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Infrastructure.Extensions
{
    public static class RepositoriesConfiguration
    {
        public static void RegisterRepositories(this IServiceCollection serviceCollection) 
        {
            serviceCollection.AddScoped<IActorRepository, ActorRepository>();
            serviceCollection.AddScoped<ICountryRepository, CountryRepository>();
            serviceCollection.AddScoped<IDirectorRepository, DirectorRepository>();
            serviceCollection.AddScoped<IGenreRepository, GenreRepository>();
            serviceCollection.AddScoped<ILanguageRepository, LanguageRepository>();
            serviceCollection.AddScoped<IScreenwriterRepository, ScreenwriterRepository>();
        }
    }
}
