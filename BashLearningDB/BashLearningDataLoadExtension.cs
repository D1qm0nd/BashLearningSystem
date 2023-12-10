using System.Globalization;
using BashDataBaseModels;
using CsvHelper;
using CsvHelper.Configuration;

namespace BashLearningDB;

public static class BashLearningDataLoadExtension
{
    public static bool LoadDataFromCSV(this BashLearningContext context, string path, CsvConfiguration configuration)
    {
        static bool Check<T>(IEnumerable<T>? enumerable)
        {
            if (enumerable != null)
                return false;
            return true;
        }

        static void LoadEntities<T>(CsvReader reader, BashLearningContext context)
        {
            var entity = reader.GetRecords<T>();
            if (Check(entity)) 
                context.AddRangeAsync(entity);
        }

        if (!path.EndsWith(".csv"))
            return false;
        
        using (var reader = new StreamReader(path))
        using (var csvReader = new CsvReader(reader, configuration))
        {
            LoadEntities<User>(csvReader, context);
            LoadEntities<Admin>(csvReader, context);
            LoadEntities<Theme>(csvReader, context);
            LoadEntities<Command>(csvReader, context);
            LoadEntities<CommandAttribute>(csvReader, context);
            LoadEntities<Read>(csvReader, context);
        }
        context.SaveChanges();
        return true;
    }
    
}