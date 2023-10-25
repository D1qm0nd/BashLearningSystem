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
        var context = DbContextFactory<BashLearningContext>.CreateContext() as IDataContext;
        context.GetEntity<Theme>();
    }

    [Test]
    public void MigrateOrCreateDB()
    {
        var context = DbContextFactory<BashLearningContext>.CreateContext();
        context.Migrate();
    }

    [Test]
    public void PostgresTest()
    {
        var context = DbContextFactory<BashLearningContext>.CreateContext() as IDataContext;
        var acc = context.GetEntity<Account>().First();
        acc.Image = new byte[] { 0, 12, 231, 132, 43, 22 };
        var query = acc.GetPostgresQuery();
    }

    [Test]
    public void DropDB()
    {
        var context = DbContextFactory<BashLearningContext>.CreateContext();
        context.Drop();
    }
}