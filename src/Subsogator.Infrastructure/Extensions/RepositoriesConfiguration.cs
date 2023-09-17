using Data.DataAccess.Repositories.Implementation;
using Data.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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
            serviceCollection.AddScoped<IFilmProductionRepository, FilmProductionRepository>();
            serviceCollection.AddScoped<ISubtitlesRepository, SubtitlesRepository>();
            serviceCollection.AddScoped<IFilmProductionGenreRepository, FilmProductionGenreRepository>();
            serviceCollection.AddScoped<IFilmProductionActorRepository, FilmProductionActorRepository>();
            serviceCollection.AddScoped<IFilmProductionDirectorRepository, FilmProductionDirectorRepository>();
            serviceCollection.AddScoped<IFilmProductionScreenwriterRepository, FilmProductionScreenwriterRepository>();
            serviceCollection.AddScoped<ICommentRepository, CommentRepository>();
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
