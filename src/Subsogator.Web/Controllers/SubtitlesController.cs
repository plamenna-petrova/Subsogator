using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data.DataAccess;
using Data.DataModels.Entities;

namespace Subsogator.Web.Controllers
{
    public class SubtitlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubtitlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subtitles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Subtitles.Include(s => s.FilmProduction);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Subtitles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitles = await _context.Subtitles
                .Include(s => s.FilmProduction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subtitles == null)
            {
                return NotFound();
            }

            return View(subtitles);
        }

        // GET: Subtitles/Create
        public IActionResult Create()
        {
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Id");
            return View();
        }

        // POST: Subtitles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FilmProductionId,Id,CreatedOn,ModifiedOn")] Subtitles subtitles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subtitles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Id", subtitles.FilmProductionId);
            return View(subtitles);
        }

        // GET: Subtitles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitles = await _context.Subtitles.FindAsync(id);
            if (subtitles == null)
            {
                return NotFound();
            }
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Id", subtitles.FilmProductionId);
            return View(subtitles);
        }

        // POST: Subtitles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,FilmProductionId,Id,CreatedOn,ModifiedOn")] Subtitles subtitles)
        {
            if (id != subtitles.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subtitles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubtitlesExists(subtitles.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmProductionId"] = new SelectList(_context.FilmProductions, "Id", "Id", subtitles.FilmProductionId);
            return View(subtitles);
        }

        // GET: Subtitles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subtitles = await _context.Subtitles
                .Include(s => s.FilmProduction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subtitles == null)
            {
                return NotFound();
            }

            return View(subtitles);
        }

        // POST: Subtitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var subtitles = await _context.Subtitles.FindAsync(id);
            _context.Subtitles.Remove(subtitles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubtitlesExists(string id)
        {
            return _context.Subtitles.Any(e => e.Id == id);
        }
    }
}
