using System;
using System.Text.Json;
using EncryptModule;
using NUnit.Framework;

namespace Tests.Encrypting;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GenerateGUID()
    {
        Console.WriteLine(Guid.NewGuid());
    }

    [Test]
    public void Test1()
    {
        var sentence = "Hello world!";
        var alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?@#$%^&*()0123456789 ";
        var key = new[] { 1 };
        var crypto = new Cryptograph(key: key, alphabet: alphabet);
        var a = crypto.Coding(sentence);
        var b = crypto.DeCoding(a);
        Console.WriteLine($"{sentence}\n{a}\n{b}");
    }

    [Test]
    public void TestValues()
    {
        var a = JsonSerializer.Serialize(new CryptographValues()
        {
            Key = new[] { 7, 5, 10, 32, 41 },
            Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?@#$%^&*()0123456789 ",
        });
        //"{\"Key\":[7,5,10,32,41],\"Alphabet\":\"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?@#$%^\u0026*()0123456789 \"}";

    }
}