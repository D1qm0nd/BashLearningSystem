using System.Diagnostics;
using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Site.Models;

namespace Site.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BashLearningContext _context;
    private readonly Session<User> _session;

    public HomeController(ILogger<HomeController> logger, BashLearningContext context, Session<User> session)
    {
        _logger = logger;
        _context = context;
        _session = session;
    }

    public IActionResult Index()
    {
        var view = View(_session);
        var themes = _context.Themes
            .Include(nameof(Theme.Commands)).OrderBy(e => e.Name).ToList();
        if (_session.Data != null)
        {
            view.ViewData["AccountData"] = _session.Data.FullName();
            var history = _context.Reads
                .Include(nameof(Read.Theme)).Where(read => read.UserId == _session.Data.UserId).ToList();
            view.ViewData["ThemeHistory"] = history;
        }
        else
            view.ViewData["ThemeHistory"] = null;
        view.ViewData["ThemeList"] = themes;
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

