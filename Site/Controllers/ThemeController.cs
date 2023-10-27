using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Site.Controllers;

public class ThemeController : Controller
{
    private readonly BashLearningContext _context;
    private readonly Session<User> _session;

    public ThemeController(BashLearningContext context, Session<User> session)
    {
        _context = context;
        _session = session;
    }

    [HttpGet("Theme/{id}")]
    public IActionResult Theory(Guid? id)
    {
        var theme = _context.Themes
            .Include(t => t.Commands)
                .ThenInclude(c => c.Attributes)
                    .FirstOrDefault(t => t.ThemeId == id);
        var view = View(_session);
        view.ViewData["AccountData"] = _session?.Data?.FullName();
        view.ViewData["Theme"] = theme;
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }
}