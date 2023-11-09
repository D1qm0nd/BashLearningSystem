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
    public RegisterController(BashLearningContext context, Session<User> session) : base(context: context, session: session)
    {
    }

    // GET
    public IActionResult Index()
    {
        return View(_session);
    }


    [HttpPost]
    public IActionResult Index([Bind("Surname", "Name", "Middlename", "Login", "Password")] User user)
    {
        
        var env_val = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        
        if (env_val == null)
            throw new EnvironmentVariableExistingException("BashLearningPrivateKey");
        
        var crypt_values = JsonSerializer.Deserialize<CryptographValues>(env_val);
        if (crypt_values == null)
            throw new ArgumentException("Check input data for correct");
        
        var cryptograph = new Cryptograph(key: crypt_values.Key, alphabet: crypt_values.Alphabet);
        
        user.Login = cryptograph.Coding(user.Login);
        user.Password = cryptograph.Coding(user.Password);
        
        //TODO: Account existing check!!!
        if (_context.Users.FirstOrDefault(u => u.Login == user.Login) != null)
            return KickAction();

        _context.Register(new UserValidator(),user);
        _context.SaveRepositoryChanges();
        _session.Data = user;
        return RedirectToAction("Index","Home");
        // }
    }
}