using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;
using Subsogator.Business.Services.Favourites;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Models.Favourites.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Subsogator.Business.Services.Subtitles;
using Subsogator.Common.Helpers;

namespace Subsogator.Web.Controllers
{
    [Authorize]
    public class FavouritesController : BaseController
    {
        private readonly IFavouritesService _favouritesService;

        private readonly ISubtitlesService _subtitlesService;

        private readonly IUnitOfWork _unitOfWork;

        public FavouritesController(
            IFavouritesService favouritesService, 
            ISubtitlesService subtitlesService, 
            IUnitOfWork unitOfWork
        )
        {
            _favouritesService = favouritesService;
            _subtitlesService = subtitlesService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(
            string sortOrder,
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<UserFavouritesViewModel> allUserFavouritesViewModel = _favouritesService
                .GetAllUserFavourites(userId);

            ViewData["CurrentSort"] = sortOrder;
            ViewData["SubtitlesNameSort"] = string.IsNullOrEmpty(sortOrder)
                ? "subtitles_name_descending"
                : "";
            ViewData["FilmProductionTitleSort"] = sortOrder == "film_production_title_ascending"
                ? "film_production_title_descending"
                : "film_production_title_ascending";
            ViewData["UploaderNameSort"] = sortOrder == "uploader_name_ascending"
                ? "uploader_name_descending"
                : "uploader_name_ascending";

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["FavouritesSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allUserFavouritesViewModel = allUserFavouritesViewModel
                        .Where(aufvm =>
                            aufvm.Name.ToLower().Contains(searchTerm.ToLower()) ||
                            aufvm.RelatedFilmProduction.Title.ToLower().Contains(searchTerm.ToLower()) ||
                            aufvm.UploaderUserName.ToLower().Contains(searchTerm.ToLower())
                        );
            }

            allUserFavouritesViewModel = sortOrder switch
            {
                "subtitles_name_descending" => allUserFavouritesViewModel
                        .OrderByDescending(aufvm => aufvm.Name),
                "film_production_title_ascending" => allUserFavouritesViewModel
                        .OrderBy(aufvm => aufvm.RelatedFilmProduction.Title),
                "film_production_title_descending" => allUserFavouritesViewModel
                        .OrderByDescending(aufvm => aufvm.RelatedFilmProduction.Title),
                "uploader_name_ascending" => allUserFavouritesViewModel
                        .OrderBy(aufvm => aufvm.UploaderUserName),
                "uploader_name_descending" => allUserFavouritesViewModel
                        .OrderByDescending(aufvm => aufvm.UploaderUserName),
                _ => allUserFavouritesViewModel.OrderBy(aufvm => aufvm.Name)
            };

            if (pageSize == null)
            {
                pageSize = 3;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var userFavouritesPaginatedList = PaginatedList<UserFavouritesViewModel>
                .Create(allUserFavouritesViewModel, pageNumber ?? 1, (int)pageSize);

            return View(userFavouritesPaginatedList);
        }

        public IActionResult RemoveFromFavourites(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var favouritesToRemove = _favouritesService.FindFavourites(userId, id);
            var favouriteSubtitles = _subtitlesService.FindSubtitles(favouritesToRemove.SubtitlesId);

            _favouritesService.RemoveFromFavourites(favouritesToRemove);

            bool isUserFavouritesDeleted = _unitOfWork.CommitSaveChanges();

            if (!isUserFavouritesDeleted)
            {
                TempData["FavouritesInfoMessage"] = $"Error, couldn't remove {favouriteSubtitles.Name} from favourites";

                return RedirectToAction(nameof(Index));
            }

            TempData["FavouritesInfoMessage"] = $"Success. Removed from {favouriteSubtitles.Name} favourites";

            return RedirectToIndexActionInCurrentController();
        }
    }
}
