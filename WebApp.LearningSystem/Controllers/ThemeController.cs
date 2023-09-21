using Lib.DataBases;
using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;

namespace WebApp.LearningSystem.Controllers;

public class ThemeController : Controller
{
    private BusinessViewModel _businessModel;


    // GET

    public ThemeController(BusinessViewModel model)
    {
        _businessModel = model;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(_businessModel);
    }

    // TODO: Theme by id
    // public IActionResult Index(uint id)
    // {
    //     _businessModel.ThemeId = id;
    //     return View(_businessModel);
    // }
}