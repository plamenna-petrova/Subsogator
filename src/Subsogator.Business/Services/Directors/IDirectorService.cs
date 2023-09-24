using Data.DataModels.Entities;
using Subsogator.Web.Models.Directors.BindingModels;
using Subsogator.Web.Models.Directors.ViewModels;
using System.Collections.Generic;

namespace Subsogator.Business.Services.Directors
{
    public interface IDirectorService
    {
        IEnumerable<AllDirectorsViewModel> GetAllDirectors();

        CreateDirectorBindingModel GetDirectorCreatingDetails();

        DirectorDetailsViewModel GetDirectorDetails(string directorId);

        bool CreateDirector(CreateDirectorBindingModel createDirectorBindingModel, string[] selectedFilmProductions, string currentUserName);

        EditDirectorBindingModel GetDirectorEditingDetails(string directorId);

        bool EditDirector(EditDirectorBindingModel editDirectorBindingModel, string[] selectedFilmProductions, string currentUserName);

        DeleteDirectorViewModel GetDirectorDeletionDetails(string directorId);

        void DeleteDirector(Director director);

        Director FindDirector(string directorId);
    }
}
