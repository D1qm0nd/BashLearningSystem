using BashDataBase;
using LearningSite.WebApp.BusinessModels;
using LearningSite.WebApp.Models;
using Lib.DataBases;
using Microsoft.AspNetCore.Mvc;

namespace LearningSite.WebApp.Controllers;

public class ThemeController : Controller
{
    private LearningSession _session;
    private IDataContext _context;

    // GET

    public ThemeController(LearningSession session, BashLearningContext context)
    {
        _session = session;
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Theme/{id}")]
    public IActionResult ReadTheme(Guid? id)
    {
        if (id == null)
            _session.SetCurrentThemeIdDefault();
        else _session.SetCurrentThemeId((Guid)id);
        return View(new ThemeViewModel(_session,_context));
    }
}
    