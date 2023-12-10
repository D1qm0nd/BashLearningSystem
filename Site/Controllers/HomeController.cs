using System.Diagnostics;
using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Site.Controllers.Abstract;
using Site.Models;

namespace Site.Controllers;

public class HomeController : PermissionNeededController
{
    private readonly ILogger<HomeController> _logger;

    private readonly string? _data_api_url;

    public HomeController(ILogger<HomeController> logger, BashLearningContext context, Session<User> session, Cryptograph cryptoGraph) : base(
        context: context, session: session, new AuthorizationService(context,cryptoGraph))
    {
        _logger = logger;
        _data_api_url = Environment.GetEnvironmentVariable("DATA_API_URL");
        if (_data_api_url == null)
            throw new EnvironmentVariableExistingException("DATA_API_URL");
    }

    public IActionResult Index()
    {
        var view = View(_session);
        view.ViewData["DATA_API_URL"] = _data_api_url;
        return view;
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}