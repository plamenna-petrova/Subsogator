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
        private readonly IDirectorRepository _directorRepository;

        public DirectorService(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        public IEnumerable<AllDirectorsViewModel> GetAllDirectors()
        {
            List<AllDirectorsViewModel> allDirectors = _directorRepository
                 .GetAllAsNoTracking()
                 .Select(d => new AllDirectorsViewModel
                 {
                     Id = d.Id,
                     FirstName = d.FirstName,
                     LastName = d.LastName
                 })
                 .ToList();

            return allDirectors;
        }

        public DirectorDetailsViewModel GetDirectorDetails(string directorId)
        {
            var singleDirector = FindDirector(directorId);

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
            Director directorToCreate = new Director
            {
                FirstName = createDirectorBindingModel.FirstName,
                LastName = createDirectorBindingModel.LastName,
            };

            var allDirectors = _directorRepository.GetAllAsNoTracking();

            if (_directorRepository.Exists(allDirectors, directorToCreate))
            {
                return false;
            }

            _directorRepository.Add(directorToCreate);

            return true;
        }

        public EditDirectorBindingModel GetDirectorEditingDetails(string directorId)
        {
            var directorToEdit = FindDirector(directorId);

            if (directorToEdit == null)
            {
                return null;
            }

            var directorToEditDetails = new EditDirectorBindingModel
            {
                Id = directorToEdit.Id,
                FirstName = directorToEdit.FirstName,
                LastName = directorToEdit.LastName
            };

            return directorToEditDetails;
        }

        public bool EditDirector(EditDirectorBindingModel editDirectorBindingModel)
        {
            var directorToUpdate = FindDirector(editDirectorBindingModel.Id);

            directorToUpdate.FirstName = editDirectorBindingModel.FirstName;
            directorToUpdate.LastName = editDirectorBindingModel.LastName;

            var filteredDirectors = _directorRepository
                .GetAllAsNoTracking()
                .Where(d => !d.Id.Equals(directorToUpdate.Id))
                .AsQueryable();

            if (_directorRepository.Exists(filteredDirectors, directorToUpdate))
            {
                return false;
            }

            _directorRepository.Update(directorToUpdate);

            return true;
        }

        public DeleteDirectorViewModel GetDirectorDeletionDetails(string directorId)
        {
            var directorToDelete = FindDirector(directorId);

            if (directorToDelete == null)
            {
                return null;
            }

            var directorToDeleteDetails = new DeleteDirectorViewModel
            {
                FirstName = directorToDelete.FirstName,
                LastName = directorToDelete.LastName
            };

            return directorToDeleteDetails;
        }

        public void DeleteDirector(Director director)
        {
            _directorRepository.Delete(director);
        }

        public Director FindDirector(string directorId)
        {
            return _directorRepository.GetById(directorId);
        }
    }
}

