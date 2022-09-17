using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Subsogator.Web.Models.Directors;
using Subsogator.Web.Models.Directors.BindingModels;
using Subsogator.Web.Models.Directors.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.Directors
{
    public class DirectorService: IDirectorService
    {
        private readonly IDirectorRepository _actorRepository;

        public DirectorService(IDirectorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public IEnumerable<AllDirectorsViewModel> GetAllDirectors()
        {
            List<AllDirectorsViewModel> allDirectors = _actorRepository
                 .GetAllAsNoTracking()
                 .Select(a => new AllDirectorsViewModel
                 {
                     Id = a.Id,
                     FirstName = a.FirstName,
                     LastName = a.LastName
                 })
                 .ToList();

            return allDirectors;
        }

        public DirectorDetailsViewModel GetDirectorDetails(string actorId)
        {
            var singleDirector = FindDirector(actorId);

            if (singleDirector == null)
            {
                return null;
            }

            var singleDirectorDetails = new DirectorDetailsViewModel
            {
                Id = singleDirector.Id,
                FirstName = singleDirector.FirstName,
                LastName = singleDirector.LastName,
                CreatedOn = singleDirector.CreatedOn,
                ModifiedOn = singleDirector.ModifiedOn
            };

            return singleDirectorDetails;
        }

        public bool CreateDirector(CreateDirectorBindingModel createDirectorBindingModel)
        {
            Director actorToCreate = new Director
            {
                FirstName = createDirectorBindingModel.FirstName,
                LastName = createDirectorBindingModel.LastName,
            };

            var allDirectors = _actorRepository.GetAllAsNoTracking();

            if (_actorRepository.Exists(allDirectors, actorToCreate))
            {
                return false;
            }

            _actorRepository.Add(actorToCreate);

            return true;
        }

        public EditDirectorBindingModel GetDirectorEditingDetails(string actorId)
        {
            var actorToEdit = FindDirector(actorId);

            if (actorToEdit == null)
            {
                return null;
            }

            var actorToEditDetails = new EditDirectorBindingModel
            {
                Id = actorToEdit.Id,
                FirstName = actorToEdit.FirstName,
                LastName = actorToEdit.LastName
            };

            return actorToEditDetails;
        }

        public bool EditDirector(EditDirectorBindingModel editDirectorBindingModel)
        {
            var actorToUpdate = FindDirector(editDirectorBindingModel.Id);

            actorToUpdate.FirstName = editDirectorBindingModel.FirstName;
            actorToUpdate.LastName = editDirectorBindingModel.LastName;

            var filteredDirectors = _actorRepository
                .GetAllAsNoTracking()
                .Where(a => !a.Id.Equals(actorToUpdate.Id))
                .AsQueryable();

            if (_actorRepository.Exists(filteredDirectors, actorToUpdate))
            {
                return false;
            }

            _actorRepository.Update(actorToUpdate);

            return true;
        }

        public DeleteDirectorViewModel GetDirectorDeletionDetails(string actorId)
        {
            var actorToDelete = FindDirector(actorId);

            if (actorToDelete == null)
            {
                return null;
            }

            var actorToDeleteDetails = new DeleteDirectorViewModel
            {
                FirstName = actorToDelete.FirstName,
                LastName = actorToDelete.LastName
            };

            return actorToDeleteDetails;
        }

        public void DeleteDirector(Director actor)
        {
            _actorRepository.Delete(actor);
        }

        public Director FindDirector(string actorId)
        {
            return _actorRepository.GetById(actorId);
        }
    }
}

