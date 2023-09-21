using System.Diagnostics;
using BashDataBase;
using DataModels;
using Lib.DataBases;
using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;
using WebApp.LearningSystem.Models;

namespace WebApp.LearningSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private BusinessViewModel _bussinesViewModel;
    

    public HomeController(ILogger<HomeController> logger, BusinessViewModel bussinesModel)
    {
        _logger = logger;
        _bussinesViewModel = bussinesModel;
    }

    public IActionResult Index()
    {
        return View(_bussinesViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult ReadTheme(Theme? theme)
    {
        var a = theme;
        return new OkResult();
        // return Redirect("/Home");
    }
}