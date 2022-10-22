using Data.DataModels.Entities;
using Microsoft.AspNetCore.Http;
using Subsogator.Web.Models.FilmProductions.BindingModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.FilmProductions
{
    public interface IFilmProductionService
    {
        List<FilmProduction> GetAllFilmProductions();

        IEnumerable<AllFilmProductionsViewModel> GetAllFilmProductionsWithRelatedData();

        FilmProductionFullDetailsViewModel GetFilmProductionDetails(string filmProductionId);

        CreateFilmProductionBindingModel GetFilmProductionCreatingDetails();

        void CreateFilmProduction(CreateFilmProductionBindingModel createFilmProductionBindingModel,
            string[] selectedGenres, string[] selectedActors, string[] selectedDirectors, string[] selectedScreenwriters);

        EditFilmProductionBindingModel GetFilmProductionEditingDetails(string filmProductionId);

        void EditFilmProduction(
            EditFilmProductionBindingModel editFilmProductionBindingModel,
            string[] selectedGenres, string[] selectedActors, string[] selectedDirectors, string[] selectedScreenwriters
        );

        DeleteFilmProductionViewModel GetFilmProductionDeletionDetails(string filmProductionId);

        void DeleteFilmProduction(FilmProduction filmProduction);

        FilmProduction FindFilmProduction(string filmProductionId);
    }
}
