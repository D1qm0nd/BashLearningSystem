using System.ComponentModel.DataAnnotations;
using DataModels;
using NUnit.Framework;

namespace Tests.DataModels;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void AccountValidate()
    {
        var account = new Account();
        var a = account.Validate(new ValidationContext(account));
        Assert.Pass();
    }
}