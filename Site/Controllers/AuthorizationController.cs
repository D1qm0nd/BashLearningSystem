using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using EncryptModule;
using Microsoft.AspNetCore.Mvc;
using Site.Controllers.Abstract;
using Exceptions;

namespace Site.Controllers;

public class AuthorizationController : PermissionNeededController
{
    public AuthorizationController(BashLearningContext context, Session<User> session) : base(context, session)
    {
    }

    public IActionResult Index()
    {
        return View(_session);
    }

    public IActionResult Login([Bind("Login")] string? login, [Bind("Password")] string? password)
    {
        var env_val = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        if (env_val == null)
            throw new EnvironmentVariableExistingException("BashLearningPrivateKey");

        var crypt_values = JsonSerializer.Deserialize<CryptographValues>(env_val);
        var cryptograph = new Cryptograph(crypt_values.Key, crypt_values.Alphabet);
        login = cryptograph.Coding(login);
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
        return RedirectToAction("Index", "Home");
    }
}