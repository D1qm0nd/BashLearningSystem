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
    public class UsersTableController : PermissionNeededController
    {
        private readonly Cryptograph _cryptoGraph;

        public UsersTableController(BashLearningContext context, Session<User> session, Cryptograph cryptoGraph) : base(context,session, new AuthorizationService(context,cryptoGraph))
        {
            _cryptoGraph = cryptoGraph;
        }

        // GET: UsersTable
        [Route("data/users")]
        public async Task<IActionResult> Index()
        {
            if (!isAdmin()) return KickAction();

            if (_context.Users != null)
            {
                var view = View(await _context.Users.Where(c =>  c.IsActual == true).ToListAsync());
                view.ViewData["isAuthorized"] = _session.Data != null;
                return view;
            } 
            else return Problem("Entity set 'BashLearningContext.Users'  is null.");
        }

        // GET: UsersTable/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!isAdmin()) return KickAction();

            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: UsersTable/Create
        public IActionResult Create()
        {
            if (!isAdmin()) return KickAction();
            var view = View();
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: UsersTable/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Login,Password,Name,Surname,Middlename,IsActual")] User user)
        {
            if (!isAdmin()) return KickAction();
            
            if (true)//(ModelState.IsValid)
            {
                user.UserId = Guid.NewGuid();
                var time = DateTime.UtcNow;
                user.CreatedUTC = time;
                user.UpdatedUTC = time;
                user.Password = _cryptoGraph.Coding(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var view = View(user);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // GET: UsersTable/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (!isAdmin()) return KickAction();
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var view =  View(user);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: UsersTable/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,Login,Password,Name,Surname,Middlename,IsActual,CreatedUTC,UpdatedUTC")] User user)
        {
            if (!isAdmin()) return KickAction();
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (true)//(ModelState.IsValid)
            {
                try
                {
                    
                    user.CreatedUTC = user.CreatedUTC.ToUniversalTime();
                    user.UpdatedUTC = DateTime.UtcNow;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            var view = View(user);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // GET: UsersTable/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (!isAdmin()) return KickAction();
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            var view = View(user);
            view.ViewData["isAuthorized"] = _session.Data != null;
            return view;
        }

        // POST: UsersTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!isAdmin()) return KickAction();
            if (_context.Users == null)
            {
                return Problem("Entity set 'BashLearningContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsActual = false;
                await _context.Admins.Where(admin => admin.UserId == user.UserId && admin.IsActual)
                    .ExecuteUpdateAsync(admin => admin.SetProperty(e => e.IsActual, false));
                _context.Users.Update(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
          return (_context.Users?.Any(e => e.UserId == id && e.IsActual == true)).GetValueOrDefault();
        }
    }
}
