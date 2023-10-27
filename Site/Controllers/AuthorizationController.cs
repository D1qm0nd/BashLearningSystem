using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Microsoft.AspNetCore.Mvc;
using Site.Exceptions;

namespace Site.Controllers;

public class AuthorizationController : Controller
{
    private readonly BashLearningContext _context;
    private Session<User> _session;

    public AuthorizationController(BashLearningContext context, Session<User> session)
    {
        _context = context;
        _session = session;
    }

    public IActionResult Index()
    {
        var view = View(_session);
        view.ViewData["Context"] = _context;
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }

    public IActionResult Login([Bind("Login")] string? login, [Bind("Password")] string? password)
    {
        var env_val = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        if (env_val == null)
            throw new EnvironmentVariableExistingException("BashLearningPrivateKey");
        
        var crypt_values = JsonSerializer.Deserialize<CryptographValues>(env_val);
        var cryptograph = new Cryptograph(key: crypt_values.Key, alphabet: crypt_values.Alphabet);
        password = cryptograph.Coding(password); 
        _session.Data = _context.Users.FirstOrDefault(user => user.Login == login && user.Password == password);
        return RedirectToAction("Index", "Home");
    }

    // public IActionResult Login()
    // {
    //     var account = _businessViewModel.ContextModel.DataContext.Repository.GetEntity<Account>().FirstOrDefault();
    //     _businessViewModel.AuthorizationModel.Account = account;
    //     return Redirect("/Home");
    // }
    public IActionResult Logout()
    {
        _session.Data = null;
        return RedirectToAction("Index", "Home", _session);
    }
}