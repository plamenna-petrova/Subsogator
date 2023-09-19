using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using Subsogator.Web.Models.Genres.BindingModels;
using Subsogator.Web.Models.Genres.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Subsogator.Business.Services.Genres
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public IEnumerable<AllGenresViewModel> GetAllGenres()
        {
            List<AllGenresViewModel> allGenres = _genreRepository
                .GetAllAsNoTracking()
                    .OrderBy(g => g.Name)
                        .Select(g => new AllGenresViewModel
                        {
                            Id = g.Id,
                            Name = g.Name,
                            RelatedFilmProductions = g.FilmProductionGenres
                                .Where(fg => fg.GenreId == g.Id)
                                .Select(fpg => new FilmProductionConciseInformationViewModel
                                {
                                    Title = fpg.FilmProduction.Title
                                })
                        })
                        .ToList();

            return allGenres;
        }

        public GenreDetailsViewModel GetGenreDetails(string genreId)
        {
            var singleGenre = _genreRepository
                   .GetAllByCondition(g => g.Id == genreId)
                      .Include(g => g.FilmProductionGenres)
                        .ThenInclude(fpg => fpg.FilmProduction)
                            .FirstOrDefault();

            if (singleGenre is null)
            {
                return null;
            }

            var singleGenreDetails = new GenreDetailsViewModel
            {
                Id = singleGenre.Id,
                Name = singleGenre.Name,
                CreatedOn = singleGenre.CreatedOn,
                ModifiedOn = singleGenre.ModifiedOn,
                RelatedFilmProductions = singleGenre.FilmProductionGenres
                    .Where(fpg => fpg.GenreId == singleGenre.Id)
                    .Select(fpg => new FilmProductionDetailedInformationViewModel
                    {
                        Title = fpg.FilmProduction.Title,
                        Duration = fpg.FilmProduction.Duration,
                        ReleaseDate = fpg.FilmProduction.ReleaseDate
                    })
            };

            return singleGenreDetails;
        }

        public bool CreateGenre(CreateGenreBindingModel createGenreBindingModel)
        {
            Genre genreToCreate = new Genre
            {
                Name = createGenreBindingModel.Name
            };

            var allGenres = _genreRepository.GetAllAsNoTracking();

            if (_genreRepository.Exists(allGenres, genreToCreate))
            {
                return false;
            }

            _genreRepository.Add(genreToCreate);

            return true;
        }

        public EditGenreBindingModel GetGenreEditingDetails(string genreId)
        {
            var genreToEdit = FindGenre(genreId);

            if (genreToEdit is null)
            {
                return null;
            }

            var countyToEditDetails = new EditGenreBindingModel
            {
                Id = genreToEdit.Id,
                Name = genreToEdit.Name
            };

            return countyToEditDetails;
        }

        public bool EditGenre(EditGenreBindingModel editGenreBindingModel)
        {
            var genreToUpdate = FindGenre(editGenreBindingModel.Id);

            genreToUpdate.Name = editGenreBindingModel.Name;

            var filteredGenres = _genreRepository
                .GetAllAsNoTracking()
                    .Where(g => !g.Id.Equals(genreToUpdate.Id));

            if (_genreRepository.Exists(filteredGenres, genreToUpdate))
            {
                return false;
            }

            _genreRepository.Update(genreToUpdate);

            return true;
        }

        public DeleteGenreViewModel GetGenreDeletionDetails(string genreId)
        {
            var genreToDelete = FindGenre(genreId);

            if (genreToDelete is null)
            {
                return null;
            }

            var genreToDeleteDetails = new DeleteGenreViewModel
            {
                Name = genreToDelete.Name
            };

            return genreToDeleteDetails;
        }

        public void DeleteGenre(Genre genre)
        {
            _genreRepository.Delete(genre);
        }

        public Genre FindGenre(string genreId)
        {
            return _genreRepository.GetById(genreId);
        }
    }
}

