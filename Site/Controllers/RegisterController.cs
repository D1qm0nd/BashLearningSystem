using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using BashLearningModelsValidate;
using EncryptModule;
using Microsoft.AspNetCore.Mvc;
using Site;
using Site.Controllers;
using Site.Exceptions;

namespace WebApp.LearningSystem.Controllers;

public class RegisterController : Controller
{
    private readonly BashLearningContext _context;
    private readonly Session<User> _session;

    public RegisterController(BashLearningContext context, Session<User> session)
    {
        _session = session;
        _context = context;
    }

    // GET
    public IActionResult Index()
    {
        var view = View(_session);
        view.ViewData["isAuthorized"] = _session.Data != null;
        return view;
    }


    [HttpPost]
    public IActionResult Index([Bind("Surname", "Name", "Middlename", "Login", "Password")] User user)
    {
        //TODO: Account existing check!!!
        // if (context.Repository.GetEntity<Account>().ToList().FirstOrDefault(account) == null)
        // {
        var env_val = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        
        if (env_val == null)
            throw new EnvironmentVariableExistingException("BashLearningPrivateKey");
        
        var crypt_values = JsonSerializer.Deserialize<CryptographValues>(env_val);
        if (crypt_values == null)
            throw new ArgumentException("Check input data for correct");
        var cryptograph = new Cryptograph(key: crypt_values.Key, alphabet: crypt_values.Alphabet);
        user.Password = cryptograph.Coding(user.Password);
        _context.Register(new UserValidator(),user);
        _context.SaveRepositoryChanges();
        _session.Data = user;
        return new HomeController(null, _context, _session).Index();
        // }
    }
}