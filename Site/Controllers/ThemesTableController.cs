using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BashDataBaseModels;
using BashLearningDB;
using Site.Controllers.Abstract;

namespace Site.Controllers
{
    public class ThemesTableController : PermissionNeededController
    {
        public ThemesTableController(BashLearningContext context, Session<User> session) : base(context: context,
            session: session)
        {
        }

        // GET: ThemesTable
        [Route("data/themes")]
        public async Task<IActionResult> Index()
        {
            if (!isAdmin()) return KickAction();
            
            if (_context.Themes != null)
            {
                var view = View(await _context.Themes.ToListAsync());
                view.ViewData["isAuthorized"] = _session.Data != null;
                return view;
            }
            else return Problem("Entity set 'BashLearningContext.Themes'  is null.");
        }

        // GET: ThemesTable/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Themes == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.ThemeId == id);
            if (theme == null)
            {
                return NotFound();
            }
    
            var view = View(theme);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // GET: ThemesTable/Create
        public IActionResult Create()
        {
            if (!isAdmin()) return KickAction();

            var view = View();
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: ThemesTable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ThemeId,Name")] Theme theme)
        {
            if (!isAdmin()) return KickAction();
            
            if (true)//(ModelState.IsValid)
            {
                theme.ThemeId = Guid.NewGuid();
                var time = DateTime.UtcNow;
                theme.CreatedUTC = time;
                theme.UpdatedUTC = time;
                _context.Add(theme);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var view = View(theme);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // GET: ThemesTable/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Themes == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            var view = View(theme);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: ThemesTable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ThemeId,Name")] Theme theme)
        {
            if (!isAdmin()) return KickAction();

            if (id != theme.ThemeId)
            {
                return NotFound();
            }

            if (true)//(ModelState.IsValid)
            {
                try
                {
                    theme.UpdatedUTC = DateTime.UtcNow;
                    _context.Update(theme);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThemeExists(theme.ThemeId))
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

            var view = View(theme);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // GET: ThemesTable/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Themes == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes
                .FirstOrDefaultAsync(m => m.ThemeId == id);
            if (theme == null)
            {
                return NotFound();
            }

            var view = View(theme);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: ThemesTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!isAdmin()) return KickAction();

            if (_context.Themes == null)
            {
                return Problem("Entity set 'BashLearningContext.Themes'  is null.");
            }

            var theme = await _context.Themes.FindAsync(id);
            if (theme != null)
            {
                _context.Themes.Remove(theme);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThemeExists(Guid id)
        {
            return (_context.Themes?.Any(e => e.ThemeId == id)).GetValueOrDefault();
        }
    }
}