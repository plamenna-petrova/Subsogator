using Microsoft.Extensions.DependencyInjection;
using Subsogator.Business.Services.Actors;
using Subsogator.Business.Services.Comments;
using Subsogator.Business.Services.Countries;
using Subsogator.Business.Services.Directors;
using Subsogator.Business.Services.FilmProductions;
using Subsogator.Business.Services.Genres;
using Subsogator.Business.Services.Languages;
using Subsogator.Business.Services.Screenwriters;
using Subsogator.Business.Services.Subtitles;
using Subsogator.Business.Services.SubtitlesCatalogue;
using Subsogator.Business.Services.Users;
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
            serviceCollection.AddTransient<ILanguageService, LanguageService>();
            serviceCollection.AddTransient<IScreenwriterService, ScreenwriterService>();
            serviceCollection.AddTransient<IFilmProductionService, FilmProductionService>();
            serviceCollection.AddTransient<ISubtitlesService, SubtitlesService>();
            serviceCollection.AddTransient<ISubtitlesCatalogueService, SubtitlesCatalogueService>();
            serviceCollection.AddTransient<ICommentService, CommentService>();
            serviceCollection.AddTransient<IUserService, UserService>();
        }
    }
}
