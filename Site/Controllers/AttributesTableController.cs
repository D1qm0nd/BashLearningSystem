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

namespace Site.Controllers;

public class AttributesTableController : PermissionNeededController
{
    public AttributesTableController(BashLearningContext context, Session<User> session) : base(context, session)
    {
    }

    // GET: AttributesTable
    [Route("data/attributes")]
    public async Task<IActionResult> Index()
    {
        if (!isAdmin()) return KickAction();

        var bashLearningContext = _context.Attributes.Include(c => c.Command);
        return View(await bashLearningContext.ToListAsync());
    }

    // GET: AttributesTable/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (!isAdmin()) return KickAction();


        if (id == null || _context.Attributes == null) return NotFound();

        var commandAttribute = await _context.Attributes
            .Include(c => c.Command)
            .FirstOrDefaultAsync(m => m.AttributeId == id);
        if (commandAttribute == null) return NotFound();

        return View(commandAttribute);
    }

    // GET: AttributesTable/Create
    public IActionResult Create()
    {
        ViewData["CommandId"] = new SelectList(_context.Commands, "CommandId", "Text");
        return View();
    }

    // POST: AttributesTable/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("AttributeId,Text,Description,CommandId,IsActual,CreatedUTC,UpdatedUTC")]
        CommandAttribute commandAttribute)
    {
        if (!isAdmin()) return KickAction();

        if (true) //(ModelState.IsValid)
        {
            commandAttribute.AttributeId = Guid.NewGuid();
            commandAttribute.CreatedUTC = DateTime.UtcNow;
            commandAttribute.UpdatedUTC = commandAttribute.CreatedUTC;
            _context.Add(commandAttribute);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CommandId"] =
            new SelectList(_context.Commands, "CommandId", "Description", commandAttribute.CommandId);
        return View(commandAttribute);
    }

    // GET: AttributesTable/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (!isAdmin()) return KickAction();

        if (id == null || _context.Attributes == null) return NotFound();

        var commandAttribute = await _context.Attributes.FindAsync(id);
        if (commandAttribute == null) return NotFound();
        ViewData["CommandId"] =
            new SelectList(_context.Commands, "CommandId", "Description", commandAttribute.CommandId);
        return View(commandAttribute);
    }

    // POST: AttributesTable/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("AttributeId,Text,Description,IsActual,CommandId,CreatedUTC,UpdatedUTC")]
        CommandAttribute commandAttribute)
    {
        if (!isAdmin()) return KickAction();

        if (id != commandAttribute.AttributeId) return NotFound();

        if (true) //(ModelState.IsValid)
        {
            try
            {
                commandAttribute.CreatedUTC = commandAttribute.CreatedUTC.ToUniversalTime();
                commandAttribute.UpdatedUTC = DateTime.UtcNow;
                _context.Update(commandAttribute);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandAttributeExists(commandAttribute.AttributeId))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CommandId"] =
            new SelectList(_context.Commands, "CommandId", "Description", commandAttribute.CommandId);
        return View(commandAttribute);
    }

    // GET: AttributesTable/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (!isAdmin()) return KickAction();

        if (id == null || _context.Attributes == null) return NotFound();

        var commandAttribute = await _context.Attributes
            .Include(c => c.Command)
            .FirstOrDefaultAsync(m => m.AttributeId == id);
        if (commandAttribute == null) return NotFound();

        return View(commandAttribute);
    }

    // POST: AttributesTable/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!isAdmin()) return KickAction();

        if (_context.Attributes == null) return Problem("Entity set 'BashLearningContext.Attributes'  is null.");
        var commandAttribute = await _context.Attributes.FindAsync(id);
        if (commandAttribute != null) _context.Attributes.Remove(commandAttribute);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommandAttributeExists(Guid id)
    {
        return (_context.Attributes?.Any(e => e.AttributeId == id)).GetValueOrDefault();
    }
}