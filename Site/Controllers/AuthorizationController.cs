using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Microsoft.AspNetCore.Mvc;
using Site.Controllers.Abstract;

namespace Site.Controllers;

public class AuthorizationController : PermissionNeededController
{
    public AuthorizationController(BashLearningContext context, Session<User> session,  Cryptograph cryptoGraph) : base(context: context, session: session, new AuthorizationService(context, cryptoGraph))
    {
        
    }
    
    public IActionResult Index()
    {
        return View(_session);
    }

    public IActionResult Login([Bind("Login")] string? login, [Bind("Password")] string? password)
    {

        _session.Data = _authorizationService.Login(login, password).Result;
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        _session.Data = null;
        return RedirectToAction("Index", "Home");
    }
}