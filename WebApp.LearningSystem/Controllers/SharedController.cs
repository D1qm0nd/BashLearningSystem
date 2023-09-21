using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;

namespace WebApp.LearningSystem.Controllers;

public class SharedController : Controller
{
    private BusinessViewModel _model;
    
    public SharedController(BusinessViewModel model)
    {
        _model = model;
    }
    // GET
    public IActionResult _Layout()
    {
        return View(_model);
    }
}