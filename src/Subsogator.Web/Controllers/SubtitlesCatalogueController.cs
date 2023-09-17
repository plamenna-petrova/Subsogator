using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subsogator.Business.Services.Comments;
using Subsogator.Business.Services.Subtitles;
using Subsogator.Business.Services.SubtitlesCatalogue;
using Subsogator.Business.Transactions.Interfaces;
using Subsogator.Web.Helpers;
using Subsogator.Web.Models.Comments.BindingModels;
using Subsogator.Web.Models.SubtitlesCatalogue;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Subsogator.Web.Controllers
{
    [AllowAnonymous]
    public class SubtitlesCatalogueController : BaseController
    {
        private readonly ISubtitlesCatalogueService _subtitlesCatalogueService;

        private readonly ISubtitlesService _subtitlesService;

        private readonly ICommentService _commentService;

        private readonly IUnitOfWork _unitOfWork;

        private IWebHostEnvironment _webHostEnvironment;

        private ILogger _logger;

        public SubtitlesCatalogueController(
            ISubtitlesCatalogueService subtitlesCatalogueService,
            ISubtitlesService subtitlesService,
            ICommentService commentService,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment webHostEnvironment,
            ILogger<SubtitlesCatalogueController> logger
        )
        {
            _subtitlesCatalogueService = subtitlesCatalogueService;
            _subtitlesService = subtitlesService;
            _commentService = commentService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult Index(
            string currentFilter,
            string searchTerm,
            int? pageSize,
            int? pageNumber)
        {
            IEnumerable<AllSubtitlesForCatalogueViewModel> allSubtitlesForCatalogueViewModel =
                _subtitlesCatalogueService
                    .GetAllSubtitlesForCatalogue();

            bool isAllSubtitlesForCatalogueViewModelEmpty = allSubtitlesForCatalogueViewModel.Count() == 0;

            if (isAllSubtitlesForCatalogueViewModelEmpty)
            {
                return NotFound();
            }

            if (searchTerm != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchTerm = currentFilter;
            }

            ViewData["SubtitlesCatalogueSearchFilter"] = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                allSubtitlesForCatalogueViewModel = allSubtitlesForCatalogueViewModel
                        .Where(ascvm =>
                            ascvm.Name.ToLower().Contains(searchTerm.ToLower()) ||
                            ascvm.RelatedFilmProduction.ReleaseDate.Year.ToString().ToLower()
                                                .Contains(searchTerm.ToLower()) ||
                            ascvm.RelatedFilmProduction.CountryName.ToLower()
                                                .Contains(searchTerm.ToLower()) ||
                            ascvm.RelatedFilmProduction.LanguageName.ToLower()
                                                .Contains(searchTerm.ToLower())
                        );
            }

            if (pageSize == null)
            {
                pageSize = 5;
            }

            ViewData["CurrentPageSize"] = pageSize;

            var paginatedList = PaginatedList<AllSubtitlesForCatalogueViewModel>
                .Create(allSubtitlesForCatalogueViewModel, pageNumber ?? 1, (int)pageSize);

            return View(paginatedList);
        }

        public IActionResult Details(string id)
        {
            SubtitlesCatalogueItemDetailsViewModel subtitlesDetailsViewModel = _subtitlesCatalogueService
                .GetSubtitlesCatalogueItemDetails(id);

            if (subtitlesDetailsViewModel == null)
            {
                return NotFound();
            }

            var correspondingComments = _commentService
                    .GetAllCommentsForSubtitlesCatalogueItem(id)
                        .OrderByDescending(c => c.CreatedOn)
                            .ToList();

            subtitlesDetailsViewModel.Comments = correspondingComments;

            return View(subtitlesDetailsViewModel);
        }

        public async Task<IActionResult> DownloadSubtitles(string id)
        {
            var subtitles = _subtitlesService.GetAllToList()
                    .Where(s => s.Id == id)
                        .FirstOrDefault();

            string wwwRootPath = _webHostEnvironment.WebRootPath;

            string subtitlesDirectoryOutputPath =
                Path.Combine(wwwRootPath + @$"\archives\subtitles\{subtitles.FilmProductionId}");

            if (!Directory.Exists(subtitlesDirectoryOutputPath))
            {
                TempData["SubtitlesCatalogueErrorMessage"] =
                   "An archive for this subtitles cannot be downloaded due to lack of uploaded files";

                return RedirectToAction(nameof(Details), new { subtitles.Id });
            }
            else
            {
                var subtitlesFilePaths = Directory.GetFiles(subtitlesDirectoryOutputPath);

                var zipFileMemoryStream = new MemoryStream();

                using (ZipArchive zipArchive = new ZipArchive(
                        zipFileMemoryStream, ZipArchiveMode.Update, leaveOpen: true
                ))
                {
                    foreach (var subtitlesFilePath in subtitlesFilePaths)
                    {
                        var subtitlesFileName = Path.GetFileName(subtitlesFilePath);
                        var entry = zipArchive.CreateEntry(subtitlesFileName);

                        using (var entryStream = entry.Open())
                        using (var fileStream = System.IO.File.OpenRead(subtitlesFilePath))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }

                zipFileMemoryStream.Seek(0, SeekOrigin.Begin);

                return File(zipFileMemoryStream, "application/zip", $"{subtitles.Name}.zip");
            }
        }

        public IActionResult WriteComment(
            SubtitlesCatalogueItemDetailsViewModel subtitlesCatalogueDetailsViewModel
        )
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var createCommentBindingModel = new CreateCommentBindingModel
            {
                Content = subtitlesCatalogueDetailsViewModel.CommentContent
            };

            bool isNewCommentCreated = _commentService
                .CreateComment(createCommentBindingModel, subtitlesCatalogueDetailsViewModel.Id, userId);

            if (!isNewCommentCreated)
            {
                TempData["CommentErrorMessage"] = "Couldn't create comment!";

                return RedirectToAction(nameof(Details), new { subtitlesCatalogueDetailsViewModel.Id });
            }

            bool isNewCommentSavedToDatabase = _unitOfWork.CommitSaveChanges();

            if (!isNewCommentSavedToDatabase)
            {
                TempData["CommentErrorMessage"] = "Couldn't save comment to database!";

                return RedirectToAction(nameof(Details), new { subtitlesCatalogueDetailsViewModel.Id });
            }

            TempData["CommentSuccessMessage"] = "Comment added successfully!";

            return RedirectToAction(nameof(Details), new { subtitlesCatalogueDetailsViewModel.Id });
        }
    }
}
