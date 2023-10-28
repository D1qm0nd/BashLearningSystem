using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Site.Controllers.Abstract;

namespace Site.Controllers;

public class ThemeController : PermissionNeededController
{

    public ThemeController(BashLearningContext context, Session<User> session) : base(context: context, session:session)
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
        view.ViewData["AccountData"] = _session?.Data?.FullName();
        view.ViewData["Theme"] = theme;
        return view;
    }
}