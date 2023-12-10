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

    public bool isAuthorized() => _session.Data != null;

    public override ViewResult View()
    {
        var view = base.View();
        var authorized = isAuthorized();
        view.ViewData["isAuthorized"] = authorized;
        if (authorized)
            view.ViewData["CurrentUser"] = _session.Data.FullName();
        return view;
    }

    public override ViewResult View(string? viewName)
    {
        var view = base.View(viewName);
        var authorized = isAuthorized();
        view.ViewData["isAuthorized"] = authorized;
        if (authorized)
            view.ViewData["CurrentUser"] = _session.Data.FullName();

        return view;
    }

    public override ViewResult View(object? model)
    {
        var view = base.View(model);
        var authorized = isAuthorized();
        view.ViewData["isAuthorized"] = authorized;
        if (authorized)
            view.ViewData["CurrentUser"] = _session.Data.FullName();

        return view;
    }

    public override ViewResult View(string? viewName, object? model)
    {
        var view = base.View(viewName, model);
        var authorized = isAuthorized();
        view.ViewData["isAuthorized"] = authorized;
        if (authorized)
            view.ViewData["CurrentUser"] = _session.Data.FullName();
        return view;
    }
}