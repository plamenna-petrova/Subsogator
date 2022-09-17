using Data.DataAccess.Repositories.Interfaces;
using Data.DataModels.Entities;
using Subsogator.Web.Models.Countries.BindingModels;
using Subsogator.Web.Models.Countries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Subsogator.Business.Services.Countries
{
    public class CountryService: ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public IEnumerable<AllCountriesViewModel> GetAllCountries()
        {
            List<AllCountriesViewModel> allCountries = _countryRepository
                .GetAllAsNoTracking()
                .Select(c => new AllCountriesViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedOn = c.CreatedOn,
                    ModifiedOn = c.ModifiedOn
                })
                .ToList();

            return allCountries;
        }

        public CountryDetailsViewModel GetCountryDetails(string countryId)
        {
            var singleCountry = FindCountry(countryId);

            if (singleCountry == null)
            {
                return null;
            }

            var singleCountryDetails = new CountryDetailsViewModel
            {
                Name = singleCountry.Name,
                CreatedOn = singleCountry.CreatedOn,
                ModifiedOn = singleCountry.ModifiedOn
            };

            return singleCountryDetails;
        }

        public bool CreateCountry(CreateCountryBindingModel createCountryBindingModel)
        {
            Country countryToCreate = new Country
            {
                Name = createCountryBindingModel.Name
            };

            var allCountries = _countryRepository.GetAllAsNoTracking();

            if (_countryRepository.Exists(allCountries, countryToCreate))
            {
                return false;
            }

            _countryRepository.Add(countryToCreate);

            return true;
        }

        public EditCountryBindingModel GetCountryEditingDetails(string countryId)
        {
            var countryToEdit = FindCountry(countryId);

            if (countryToEdit == null)
            {
                return null;
            }

            var countyToEditDetails = new EditCountryBindingModel
            {
                Id = countryToEdit.Id,
                Name = countryToEdit.Name
            };

            return countyToEditDetails;
        }

        public bool EditCountry(EditCountryBindingModel editCountryBindingModel)
        {
            var countryToUpdate = FindCountry(editCountryBindingModel.Id);

            countryToUpdate.Name = editCountryBindingModel.Name;

            var filteredCountries = _countryRepository
                .GetAllAsNoTracking()
                .Where(c => !c.Id.Equals(countryToUpdate.Id));

            if (_countryRepository.Exists(filteredCountries, countryToUpdate))
            {
                return false;
            }

            _countryRepository.Update(countryToUpdate);

            return true;
        }

        public DeleteCountryViewModel GetCountryDeletionDetails(string countryId)
        {
            var countryToDelete = FindCountry(countryId);

            if (countryToDelete == null)
            {
                return null;
            }

            var countryToDeleteDetails = new DeleteCountryViewModel
            {
                Name = countryToDelete.Name
            };

            return countryToDeleteDetails;
        }

        public void DeleteCountry(Country country)
        {
            _countryRepository.Delete(country);
        }

        public Country FindCountry(string countryId)
        {
            return _countryRepository.GetById(countryId);
        }
    }
}
