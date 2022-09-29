using Data.DataModels.Entities;
using Subsogator.Web.Models.Directors;
using Subsogator.Web.Models.Directors.BindingModels;
using Subsogator.Web.Models.Directors.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Directors
{
    public interface IDirectorService
    {
        IEnumerable<AllDirectorsViewModel> GetAllDirectors();

        CreateDirectorBindingModel GetDirectorCreatingDetails();

        DirectorDetailsViewModel GetDirectorDetails(string directorId);

        bool CreateDirector(CreateDirectorBindingModel createDirectorBindingModel, string[] selectedFilmProductions);

        EditDirectorBindingModel GetDirectorEditingDetails(string directorId);

        bool EditDirector(EditDirectorBindingModel editDirectorBindingModel, string[] selectedFilmProductions);

        DeleteDirectorViewModel GetDirectorDeletionDetails(string directorId);

        void DeleteDirector(Director director);

        Director FindDirector(string directorId);
    }
}
