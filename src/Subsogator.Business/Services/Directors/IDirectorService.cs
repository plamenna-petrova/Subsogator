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

        DirectorDetailsViewModel GetDirectorDetails(string actorId);

        bool CreateDirector(CreateDirectorBindingModel createDirectorBindingModel);

        EditDirectorBindingModel GetDirectorEditingDetails(string actorId);

        bool EditDirector(EditDirectorBindingModel editDirectorBindingModel);

        DeleteDirectorViewModel GetDirectorDeletionDetails(string actorId);

        void DeleteDirector(Director actor);

        Director FindDirector(string actorId);
    }
}
