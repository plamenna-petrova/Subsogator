using Data.DataModels.Entities;
using Subsogator.Web.Models.Countries.BindingModels;
using Subsogator.Web.Models.Countries.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subsogator.Business.Services.Countries
{
    public interface ICountryService
    {
        IEnumerable<AllCountriesViewModel> GetAllCountries();

        CountryDetailsViewModel GetCountryDetails(string countryId);

        bool CreateCountry(CreateCountryBindingModel createCountryBindingModel);

        EditCountryBindingModel GetCountryEditingDetails(string countryId);

        bool EditCountry(EditCountryBindingModel editCountryBindingModel);

        DeleteCountryViewModel GetCountryDeletionDetails(string countryId);

        void DeleteCountry(Country country);

        Country FindCountry(string countryId);
    }
}
