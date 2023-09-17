using Microsoft.AspNetCore.Mvc;
using System;

namespace Subsogator.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IActionResult RedirectToIndexActionInCurrentController()
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
