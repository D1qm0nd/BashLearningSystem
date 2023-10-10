using System;
using System.Linq;
using BashDataBase;
using DataModels;
using Lib.DataBases;
using NUnit.Framework;

namespace Tests.DataBase;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestRepository()
    {
        var context = DbContextFactory<BashLearningDataContext>.CreateContext();
        context.Repository.GetEntity<Theme>();
    }

    [Test]
    public void MigrateOrCreateDB()
    {
        var context = DbContextFactory<BashLearningDataContext>.CreateContext();
        context.Repository.Migrate();
    }

    [Test]
    public void PostgresTest()
    {
        var context = DbContextFactory<BashLearningDataContext>.CreateContext();
        var acc = context.Repository.GetEntity<Account>().First();
        acc.Image = new byte[] { 0, 12, 231, 132, 43, 22 };
        var query = acc.GetPostgresQuery();
    }

    [Test]
    public void DropDB()
    {
        var context = DbContextFactory<BashLearningDataContext>.CreateContext();
        context.Repository.Drop();
    }
}