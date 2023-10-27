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
}