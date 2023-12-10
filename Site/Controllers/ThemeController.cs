using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Site.Controllers.Abstract;

namespace Site.Controllers;

public class ThemeController : PermissionNeededController
{
    public ThemeController(BashLearningContext context, Session<User> session, Cryptograph cryptoGraph) : base(context: context, session: session, new AuthorizationService(context, cryptoGraph))
    {
    }

    [HttpGet("Theme/{id}")]
    public IActionResult Theory(Guid? id)
    {
        var theme = _context.Themes
            .Include(t => t.Commands)
            .ThenInclude(c => c.Attributes)
            .FirstOrDefault(t => t.ThemeId == id);
        var view = View(_session);
        // view.ViewData["AccountData"] = _session?.Data?.FullName();
        view.ViewData["Theme"] = theme;
        if (isAuthorized())
        {
            var executor_url = Environment.GetEnvironmentVariable("COMMAND_EXECUTOR_URL");
            if (executor_url == null)
                throw new EnvironmentVariableExistingException("COMMAND_EXECUTOR_URL");
            view.ViewData["BashExecutorIP"] = executor_url;
        }

        view.ViewData["isRead"] = _session.Data != null ? ReadRecordExist(_session.Data.UserId, (Guid)id) : false;
        return view;
    }

    public bool ReadRecordExist(Guid UserId, Guid ThemeId)
    {
        return _context.Reads.FirstOrDefault(r => r.UserId == UserId && r.ThemeId == ThemeId) != null;
    }

    [HttpPost("Theme/{id}/mark-as-read")]
    public IActionResult MarkAsRead(Guid id)
    {
        if (!isAuthorized()) return KickAction();

        if (!ReadRecordExist(_session.Data.UserId, id))
        {
            AddToRead(_session.Data.UserId, id);
            // _context.Reads.Add(new Read() { UserId = _session.Data.UserId, ThemeId = (Guid)id });
            // _context.SaveChangesAsync();
        }

        return KickAction();
        // return RedirectToAction("Theory", "Theme", id);
    }

    [HttpPost("Theme/{id}/mark-as-unread")]
    public IActionResult MarkAsUnRead(Guid id)
    {
        if (!isAuthorized()) return KickAction();
        if (ReadRecordExist(_session.Data.UserId, id))
        {
            RemoveFromRead(_session.Data.UserId, id);
        }

        return KickAction();
    }

    public bool AddToRead(Guid UserId, Guid ThemeId)
    {
        _context.Reads.Add(new Read() { UserId = UserId, ThemeId = ThemeId });
        return (_context.SaveChangesAsync().Result == 1);
    }

    public bool RemoveFromRead(Guid UserId, Guid ThemeId)
    {
        var read = _context.Reads.FirstOrDefault(read => read.UserId == UserId && read.ThemeId == ThemeId);
        if (read == null)
            return false;
        _context.Reads.Remove(read);
        return (_context.SaveChangesAsync().Result == 1);
    }
}