using System;
using NUnit.Framework;

namespace Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var arr = new int[10];
        var rand = new Random();
        var num = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            num = (arr[i] = rand.Next(arr.Length));
            
            if (i == arr.Length-1)
            {
                i = 0;
            }
        }
    }
}