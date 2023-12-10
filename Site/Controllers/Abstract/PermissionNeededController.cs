using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers.Abstract;

public abstract class PermissionNeededController : SessionNeededController
{
    protected readonly BashLearningContext _context;
    protected readonly AuthorizationService _authorizationService;
    
    public PermissionNeededController(BashLearningContext context,Session<User> session, AuthorizationService authorizationService) : base(session)
    {
        _context = context;
        _authorizationService = authorizationService;
    }

    public bool isAdmin() => ValidatePermission(_session, _context);
    
    private bool ValidatePermission(Session<User> session, BashLearningContext context)
    {
        if (session.Data == null)
            return false;
        return _authorizationService.IsAdmin(session.Data).Result;
    }

    protected virtual IActionResult KickAction()
    {
        return RedirectToAction("Index", "Home");
    }

    public override ViewResult View()
    {
        var view = base.View();
        view.ViewData["isAdmin"] = isAdmin();
        return view;
    }

    public override ViewResult View(string? viewName)
    {
        var view = base.View(viewName);
        view.ViewData["isAdmin"] = isAdmin();
        return view;
    }

    public override ViewResult View(object? model)
    {
        var view = base.View(model);
        view.ViewData["isAdmin"] = isAdmin();
        return view;
    }

    public override ViewResult View(string? viewName, object? model)
    {
        var view = base.View(viewName, model);
        view.ViewData["isAdmin"] = isAdmin();
        return view;
    }
}