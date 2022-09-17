﻿using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Subsogator.Web.Models.Screenwriters.BindingModels;
using Subsogator.Web.Models.Screenwriters.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.Screenwriters
{
    public class ScreenwriterService: IScreenwriterService
    {
        private readonly IScreenwriterRepository _screenwriterRepository;

        public ScreenwriterService(IScreenwriterRepository screenwriterRepository)
        {
            _screenwriterRepository = screenwriterRepository;
        }

        public IEnumerable<AllScreenwritersViewModel> GetAllScreenwriters()
        {
            List<AllScreenwritersViewModel> allScreenwriters = _screenwriterRepository
                 .GetAllAsNoTracking()
                 .Select(s => new AllScreenwritersViewModel
                 {
                     Id = s.Id,
                     FirstName = s.FirstName,
                     LastName = s.LastName
                 })
                 .ToList();

            return allScreenwriters;
        }

        public ScreenwriterDetailsViewModel GetScreenwriterDetails(string screenwriterId)
        {
            var singleScreenwriter = FindScreenwriter(screenwriterId);

            if (singleScreenwriter == null)
            {
                return null;
            }

            var singleScreenwriterDetails = new ScreenwriterDetailsViewModel
            {
                Id = singleScreenwriter.Id,
                FirstName = singleScreenwriter.FirstName,
                LastName = singleScreenwriter.LastName,
                CreatedOn = singleScreenwriter.CreatedOn,
                ModifiedOn = singleScreenwriter.ModifiedOn
            };

            return singleScreenwriterDetails;
        }

        public bool CreateScreenwriter(CreateScreenwriterBindingModel createScreenwriterBindingModel)
        {
            Screenwriter screenwriterToCreate = new Screenwriter
            {
                FirstName = createScreenwriterBindingModel.FirstName,
                LastName = createScreenwriterBindingModel.LastName,
            };

            var allScreenwriters = _screenwriterRepository.GetAllAsNoTracking();

            if (_screenwriterRepository.Exists(allScreenwriters, screenwriterToCreate))
            {
                return false;
            }

            _screenwriterRepository.Add(screenwriterToCreate);

            return true;
        }

        public EditScreenwriterBindingModel GetScreenwriterEditingDetails(string screenwriterId)
        {
            var screenwriterToEdit = FindScreenwriter(screenwriterId);

            if (screenwriterToEdit == null)
            {
                return null;
            }

            var screenwriterToEditDetails = new EditScreenwriterBindingModel
            {
                Id = screenwriterToEdit.Id,
                FirstName = screenwriterToEdit.FirstName,
                LastName = screenwriterToEdit.LastName
            };

            return screenwriterToEditDetails;
        }

        public bool EditScreenwriter(EditScreenwriterBindingModel editScreenwriterBindingModel)
        {
            var screenwriterToUpdate = FindScreenwriter(editScreenwriterBindingModel.Id);

            screenwriterToUpdate.FirstName = editScreenwriterBindingModel.FirstName;
            screenwriterToUpdate.LastName = editScreenwriterBindingModel.LastName;

            var filteredScreenwriters = _screenwriterRepository
                .GetAllAsNoTracking()
                .Where(s => !s.Id.Equals(screenwriterToUpdate.Id))
                .AsQueryable();

            if (_screenwriterRepository.Exists(filteredScreenwriters, screenwriterToUpdate))
            {
                return false;
            }

            _screenwriterRepository.Update(screenwriterToUpdate);

            return true;
        }

        public DeleteScreenwriterViewModel GetScreenwriterDeletionDetails(string screenwriterId)
        {
            var screenwriterToDelete = FindScreenwriter(screenwriterId);

            if (screenwriterToDelete == null)
            {
                return null;
            }

            var screenwriterToDeleteDetails = new DeleteScreenwriterViewModel
            {
                FirstName = screenwriterToDelete.FirstName,
                LastName = screenwriterToDelete.LastName
            };

            return screenwriterToDeleteDetails;
        }

        public void DeleteScreenwriter(Screenwriter screenwriter)
        {
            _screenwriterRepository.Delete(screenwriter);
        }

        public Screenwriter FindScreenwriter(string screenwriterId)
        {
            return _screenwriterRepository.GetById(screenwriterId);
        }
    }
}