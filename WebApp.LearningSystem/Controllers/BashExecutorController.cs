using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;

namespace WebApp.LearningSystem.Controllers;

public class BashExecutorController : Controller
{
    private BusinessViewModel _businessViewModel;
    
    public BashExecutorController(BusinessViewModel businessViewModel)
    {
        _businessViewModel = businessViewModel;
    }
    // GET
    public IActionResult Index()
    {
        return View(_businessViewModel);
    }
}