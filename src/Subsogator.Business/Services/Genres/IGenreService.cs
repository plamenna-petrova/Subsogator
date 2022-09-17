using Data.DataModels.Entities;
using Subsogator.Web.Models.Genres.BindingModels;
using Subsogator.Web.Models.Genres.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Genres
{
    public interface IGenreService
    {
        IEnumerable<AllGenresViewModel> GetAllGenres();

        GenreDetailsViewModel GetGenreDetails(string genreId);

        bool CreateGenre(CreateGenreBindingModel createGenreBindingModel);

        EditGenreBindingModel GetGenreEditingDetails(string genreId);

        bool EditGenre(EditGenreBindingModel editGenreBindingModel);

        DeleteGenreViewModel GetGenreDeletionDetails(string genreId);

        void DeleteGenre(Genre genre);

        Genre FindGenre(string genreId);
    }
}
