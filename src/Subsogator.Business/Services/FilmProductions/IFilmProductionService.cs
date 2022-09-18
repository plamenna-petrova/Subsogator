using Data.DataModels.Entities;
using Subsogator.Web.Models.FilmProductions.BindingModels;
using Subsogator.Web.Models.FilmProductions.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.FilmProductions
{
    public interface IFilmProductionService
    {
        IEnumerable<AllFilmProductionsViewModel> GetAllFilmProductions();

        FilmProductionFullDetailsViewModel GetFilmProductionDetails(string filmProductionId);

        void CreateFilmProduction(CreateFilmProductionBindingModel createFilmProductionBindingModel);

        EditFilmProductionBindingModel GetFilmProductionEditingDetails(string filmProductionId);

        void EditFilmProduction(EditFilmProductionBindingModel editFilmProductionBindingModel);

        DeleteFilmProductionViewModel GetFilmProductionDeletionDetails(string filmProductionId);

        void DeleteFilmProduction(FilmProduction filmProduction);

        FilmProduction FindFilmProduction(string filmProductionId);
    }
}
