using BashDataBaseModels;
using Microsoft.AspNetCore.Mvc;

namespace Site.Controllers.Abstract;

public abstract class SessionNeededController : Controller
{
    protected readonly Session<User> _session;

    public SessionNeededController(Session<User> session)
    {
        _session = session;
    }

    public bool isAuthorized()
    {
        return _session.Data != null;
    }

    public override ViewResult View()
    {
        var view = base.View();
        view.ViewData["isAuthorized"] = isAuthorized();
        return view;
    }

    public override ViewResult View(string? viewName)
    {
        var view = base.View(viewName);
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }

    public override ViewResult View(object? model)
    {
        var view = base.View(model);
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }

    public override ViewResult View(string? viewName, object? model)
    {
        var view = base.View(viewName, model);
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }
}