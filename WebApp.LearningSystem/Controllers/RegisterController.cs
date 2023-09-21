using BashDataBase;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using WebApp.LearningSystem.BussinesModels;

namespace WebApp.LearningSystem.Controllers;

public class RegisterController : Controller
{
    private BusinessViewModel _model;

    public RegisterController(BusinessViewModel model)
    {
        _model = model;
    }

    // GET
    public IActionResult Index()
    {
        return View(_model);
    }


    [HttpPost]
    public IActionResult Index(BashLearningDataContext context,
        [Bind("Surname", "Name", "MiddleName", "Login", "Password")] Account account)
    {
        //TODO: Account existing check!!!
        // if (context.Repository.GetEntity<Account>().ToList().FirstOrDefault(account) == null)
        // {
            context.Repository.GetEntity<Account>().Add(account);
            context.Repository.SaveRepositoryChanges();
        // }
        return Redirect("/Home");
    }
}