using BashDataBaseModels;
using BashLearningDB;
using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers.Abstract;

public abstract class PermissionNeededController : SessionNeededController
{
    protected readonly BashLearningContext _context;

    public PermissionNeededController(BashLearningContext context,Session<User> session) : base(session)
    {
        _context = context;
    }

    public bool isAdmin() => ValidatePermission(_session, _context);
    
    private bool ValidatePermission(Session<User> session, BashLearningContext context)
    {
        if (session.Data == null)
            return false;
        return context.IsAdmin(session.Data);
    }

    protected IActionResult KickAction()
    {
        return RedirectToAction("Index", "Home");
    }

}