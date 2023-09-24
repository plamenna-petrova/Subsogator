using Data.DataModels.Entities;
using Subsogator.Web.Models.Genres.BindingModels;
using Subsogator.Web.Models.Genres.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Genres
{
    public interface IGenreService
    {
        IEnumerable<AllGenresViewModel> GetAllGenres();

        GenreDetailsViewModel GetGenreDetails(string genreId);

        bool CreateGenre(CreateGenreBindingModel createGenreBindingModel, string currentUserName);

        EditGenreBindingModel GetGenreEditingDetails(string genreId);

        bool EditGenre(EditGenreBindingModel editGenreBindingModel, string currentUserName);

        DeleteGenreViewModel GetGenreDeletionDetails(string genreId);

        void DeleteGenre(Genre genre);

        Genre FindGenre(string genreId);
    }
}
