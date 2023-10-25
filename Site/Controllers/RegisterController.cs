using System.Text.Json;
using BashDataBase;
using DataModels;
using EncryptModule;
using Microsoft.AspNetCore.Mvc;
using Site;
using Site.Controllers;
using Site.Exceptions;

namespace WebApp.LearningSystem.Controllers;

public class RegisterController : Controller
{
    private readonly BashLearningContext _context;
    private readonly Session<Account> _session;

    public RegisterController(BashLearningContext context, Session<Account> session)
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
    public IActionResult Index([Bind("Surname", "Name", "MiddleName", "Login", "Password")] Account account)
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
        account.Password = cryptograph.Coding(account.Password); 
        _context.Accounts.Add(account);
        _context.SaveRepositoryChanges();
        _session.Data = account;
        return new HomeController(null, _context, _session).Index();
        // }
    }
}