using System;
using System.Text.Json;
using BashDataBaseModels;
using BashLearningDB;
using BashLearningModelsValidate;
using EncryptModule;
using Newtonsoft.Json;
using NUnit.Framework;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.BashLearningDataBase;

public class Tests
{
    private BashLearningContext _context;
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
            _cryptograph = new Cryptograph(cryptographyValues.Key,cryptographyValues.Alphabet);
        }
    }

    [Test]
    public void Register()
    {
        var user = new User()
        {
            Login = Faker.Identification.UKNationalInsuranceNumber(),
            Name = Faker.Name.First(),
            Middlename = Faker.Name.First(),
            Surname = Faker.Name.Last(),
            Password = Faker.Identification.UKNationalInsuranceNumber()
        };
        Console.WriteLine($"Login: [{user.Login}]\nPassword: [{user.Password}]");
        user.Password = _cryptograph.Coding(user.Password);
        if (!_context.Register(new UserValidator(), user))
            Assert.Fail();
    }
}