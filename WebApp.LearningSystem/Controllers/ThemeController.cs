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
        return View();
    }

    [HttpGet("Theme/{id}")]
    public IActionResult ReadTheme(Guid? id)
    {
        _businessModel.CurrentThemeId = id;
        return View(_businessModel);
    }
}
    