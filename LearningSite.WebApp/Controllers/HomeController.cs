using System.Diagnostics;
using BashDataBase;
using DataModels;
using LearningSite.WebApp.BusinessModels;
using Microsoft.AspNetCore.Mvc;
using LearningSite.WebApp.Models;
using Lib.DataBases;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private HomeViewModel _model;
    private readonly BashLearningContext _context;


    public HomeController(ILogger<HomeController> logger, LearningSession session, BashLearningContext context)
    {
        _model = new HomeViewModel(session);
        _context = context;
    }

    public IActionResult Index()
    {
        if (_model != null)
        {
            var view = View(_model);
            // view.ViewData["Context"] = context;
            view.ViewData["Themes"] = _context.Themes
                .Include(nameof(Theme.Commands)).OrderBy(e => e.Name).ToList();
            var history = null as List<ExercisesHistory>;
            if (_model.Session.Account != null)
            {
                history = _context.ExercisesHistory
                    .Include(nameof(ExercisesHistory.Exercise)).ToList();
            }

            view.ViewData["History"] = history;
            return view;
        }

        return null;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}