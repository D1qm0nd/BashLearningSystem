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
    private BusinessViewModel _businessViewModel;
    

    public HomeController(ILogger<HomeController> logger, BusinessViewModel businessModel)
    {
        _logger = logger;
        _businessViewModel = businessModel;
    }

    public IActionResult Index()
    {
        return View(_businessViewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}