using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Site.Controllers.Abstract;

namespace Site.Controllers
{
    public class CommandsTableController : PermissionNeededController
    {

        public CommandsTableController(BashLearningContext context, Session<User> session, Cryptograph cryptoGraph) : base(context: context, session: session, new AuthorizationService(context,cryptoGraph))
        {
        }

        // GET: CommandsTable
        [Route("data/commands")]
        public async Task<IActionResult> Index()
        {
            if (!isAdmin()) return KickAction();
            
            var bashLearningContext = _context.Commands.Include(c => c.Theme).Include(c => c.Attributes).Where(c =>  c.IsActual == true).OrderBy(c => c.Theme.ThemeId);
            return View(await bashLearningContext.ToListAsync());
        }

        // GET: CommandsTable/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Commands == null)
            {
                return NotFound();
            }

            var command = await _context.Commands
                .Include(c => c.Theme)
                .FirstOrDefaultAsync(m => m.CommandId == id);
            if (command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        // GET: CommandsTable/Create
        public IActionResult Create()
        {
            if (!isAdmin()) return KickAction();

            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "Name");
            return View();
        }

        // POST: CommandsTable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CommandId,Text,Description,ThemeId,IsActual,CreatedUTC,UpdatedUTC")] Command command)
        {
            if (!isAdmin()) return KickAction();

            if (true) //(ModelState.IsValid)
            {
                command.CommandId = Guid.NewGuid();
                _context.Add(command);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "Name", command.ThemeId);
            return View(command);
        }

        // GET: CommandsTable/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Commands == null)
            {
                return NotFound();
            }

            var command = await _context.Commands.FindAsync(id);
            if (command == null)
            {
                return NotFound();
            }
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "Name", command.ThemeId);
            return View(command);
        }

        // POST: CommandsTable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CommandId,Text,Description,ThemeId,IsActual,CreatedUTC")] Command command)
        {
            if (!isAdmin()) return KickAction();

            if (id != command.CommandId)
            {
                return NotFound();
            }

            if (true) //(ModelState.IsValid)
            {
                try
                {
                    command.CreatedUTC = command.CreatedUTC.ToUniversalTime();
                    // command.UpdatedUTC = DateTime.UtcNow;
                    _context.Update(command);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommandExists(command.CommandId))
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
            ViewData["ThemeId"] = new SelectList(_context.Themes, "ThemeId", "Name", command.ThemeId);
            return View(command);
        }

        // GET: CommandsTable/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Commands == null)
            {
                return NotFound();
            }

            var command = await _context.Commands
                .Include(c => c.Theme)
                .FirstOrDefaultAsync(m => m.CommandId == id);
            if (command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        // POST: CommandsTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!isAdmin()) return KickAction();

            if (_context.Commands == null)
            {
                return Problem("Entity set 'BashLearningContext.Commands'  is null.");
            }
            var command = await _context.Commands.FindAsync(id);
            if (command != null)
            {
                command.IsActual = false;
                _context.Commands.Update(command);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommandExists(Guid id)
        {
          return (_context.Commands?.Any(e => e.CommandId == id && e.IsActual == true)).GetValueOrDefault();
        }
    }
}
