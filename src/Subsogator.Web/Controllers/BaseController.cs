using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
