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
    public class AdminsTableController : PermissionNeededController
    {
        public AdminsTableController(BashLearningContext context, Session<User> session) : base(context, session)
        {
        }

        // GET: AdminsTableController
        [Route("data/admins")]
        public async Task<IActionResult> Index()
        {
            if (!isAdmin()) return KickAction();
            var bashLearningContext = _context.Admins.Include(a => a.User);
            return View(await bashLearningContext.ToListAsync());
        }

        // GET: AdminsTableController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: AdminsTableController/Create
        public IActionResult Create()
        {
            if (!isAdmin()) return KickAction();

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Login");
            return View();
        }

        // POST: AdminsTableController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,UserId,IsActual")] Admin admin)
        {
            if (!isAdmin()) return KickAction();

            if (true) //(ModelState.IsValid)
            {
                var time = DateTime.UtcNow;
                admin.UpdatedUTC = time;
                admin.CreatedUTC = time;
                admin.AdminId = Guid.NewGuid();
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Login", admin.UserId);
            
            return View(admin);
        }

        // GET: AdminsTableController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Login", admin.UserId);
            return View(admin);
        }

        // POST: AdminsTableController/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("AdminId,UserId,IsActual,CreatedUTC,UpdatedUTC")] Admin admin)
        {
            if (!isAdmin()) return KickAction();

            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (true) //(ModelState.IsValid)
            {
                try
                {
                    admin.CreatedUTC = admin.CreatedUTC.ToUniversalTime();
                    admin.UpdatedUTC = DateTime.UtcNow;
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
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

            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Login", admin.UserId);
            return View(admin);
        }

        // GET: AdminsTableController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Admins == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // POST: AdminsTableController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!isAdmin()) return KickAction();

            if (_context.Admins == null)
            {
                return Problem("Entity set 'BashLearningContext.Admins'  is null.");
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(Guid id)
        {
            return (_context.Admins?.Any(e => e.AdminId == id)).GetValueOrDefault();
        }
    }
}