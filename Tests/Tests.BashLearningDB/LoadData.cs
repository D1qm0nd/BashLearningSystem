using System.Globalization;
using System.IO;
using System.Text;
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
        var path = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}BashLearningDB{Path.DirectorySeparatorChar}Data";
        _context.LoadDataFromCSV(path, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = ",",
            Encoding = Encoding.UTF8,
            PrepareHeaderForMatch = (args) =>
            {
            var prepared = args.Header.ToLower();
            return prepared;
        },
        });
    }
}