using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using BashLearningModelsValidate;
using EncryptModule;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using Site;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.BashLearningDataBase;

public class Tests
{
    private BashLearningContext _context;
    private AuthorizationService _authorizationService;
    private Cryptograph _cryptograph;
    
    [SetUp]
    public void Setup()
    {
        _context = new BashLearningContext();
        var variable = Environment.GetEnvironmentVariable("BashLearningPrivateKey");
        if (variable != null)
        {
            var cryptographyValues = JsonSerializer.Deserialize<CryptographValues>(variable);
            if (cryptographyValues == null)
                throw new ArgumentException();
            _cryptograph = new Cryptograph(cryptographyValues.Key, cryptographyValues.Alphabet);
            _authorizationService = new AuthorizationService(_context, _cryptograph);
        }
    }

    [Test]
    public void _1_Register_Admin_User()
    {
        var user = new User()
        {
            Login = "admin",
            Name = "admin",
            Middlename = "admin",
            Surname = "admin",
            Password = "admin"
        };
        Console.WriteLine($"Login: [{user.Login}]\nPassword: [{user.Password}]");

        var result = _authorizationService.Register(new UserValidator(), user).Result;
        
        if (result == null)
            Assert.Fail();
        else Assert.Pass();
    }

    [Test]
    public void _3_isAdmin()
    {
        var user = _context.Users.First();
        var res = _authorizationService.IsAdmin(user);
        Console.WriteLine(res);
    }

    [Test]
    public void _2_CreateBaseAdmin()
    {
        var user = _context.Users.FirstOrDefaultAsync(u => u.Surname == "admin" && u.Name == "admin"  && u.Middlename == "admin" && u.IsActual == true).Result;
        _context.Admins.Add(new Admin() { UserId = user.UserId, IsActual = true });
        if (_context.SaveChanges() == 1)
            Assert.Pass();
        else Assert.Fail();
    }

}