using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using BashLearningModelsValidate;
using EncryptModule;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Site;
using Site.Controllers;
using Site.Controllers.Abstract;

namespace WebApp.LearningSystem.Controllers;

public class RegisterController : PermissionNeededController
{
    private readonly AuthorizationService _authorizationService;

    public RegisterController(BashLearningContext context, Session<User> session, Cryptograph cryptoGraph) : base(context: context, session: session, new AuthorizationService(context, cryptoGraph))
    {
        _authorizationService = new AuthorizationService(context, cryptoGraph);
    }

    // GET
    public IActionResult Index()
    {
        return View(_session);
    }


    [HttpPost]
    public IActionResult Index([Bind("Surname", "Name", "Middlename", "Login", "Password")] User user)
    {
        _session.Data = _authorizationService.Register(new UserValidator(),user).Result;
        return RedirectToAction("Index","Home");
    }
}
