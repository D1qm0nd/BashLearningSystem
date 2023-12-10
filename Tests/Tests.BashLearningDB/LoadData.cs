using System.Globalization;
using BashLearningDB;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace Tests.BashLearningDataBase;

public class LoadData
{
    private BashLearningContext _context;
    
    [SetUp]
    public void Initialize()
    {
        _context = new BashLearningContext();
    }


    [Test]
    public void LoadInitialData()
    {
        _context.LoadDataFromCSV("", new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true
        });
    }
}