using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using BashDataBaseModels;
using BashDataBaseModels.CSV;
using CsvHelper;
using CsvHelper.Configuration;
using Lib.DataBases;

namespace BashLearningDB;

public static class BashLearningDataLoadExtension
{
    public static bool LoadDataFromCSV(this BashLearningContext context, string path, CsvConfiguration configuration)
    {
        static IEnumerable<T>? LoadEntities<T>(string path, CsvConfiguration configuration)
        {
            var tableName = typeof(T).Name;
            var tableAttr = typeof(T).GetCustomAttributes(typeof(TableAttribute)).FirstOrDefault();
            if (tableAttr != null)
            {
                var value = tableAttr.GetType().GetProperty("Name")?.GetValue(tableAttr);
                if (value != null)
                    tableName = (string)value;
            }
            string filePath;
            if (!path.EndsWith(Path.DirectorySeparatorChar))
                filePath = $"{path}{Path.DirectorySeparatorChar}{tableName}.csv";
            else filePath = $"{path}{tableName}.csv";

            IEnumerable<T>? entities = null;
            using (var reader = new StreamReader(filePath))
            using (var csvReader = new CsvReader(reader, configuration))
            {
                entities = csvReader.GetRecords<T>().ToList<T>();
            }

            return entities;
        }

        var csvUsers = LoadEntities<CsvUser>(path, configuration)?.ToList();
        var csvAdmins = LoadEntities<CsvAdmin>(path, configuration)?.ToList();
        var csvThemes = LoadEntities<CsvTheme>(path, configuration)?.ToList();
        var csvCommands = LoadEntities<CsvCommand>(path, configuration)?.ToList();
        var csvCommandAttributes = LoadEntities<CsvCommandAttribute>(path, configuration)?.ToList();
        var csvReads = LoadEntities<CsvRead>(path, configuration)?.ToList();

            
        if (csvUsers != null)
            context.Users.AddRange(CsvEntity.MapIEnumerable<CsvUser,User>(csvUsers));
        if (csvAdmins != null)
            context.Admins.AddRange(CsvEntity.MapIEnumerable<CsvAdmin,Admin>(csvAdmins));
        if (csvThemes != null)
            context.Themes.AddRange(CsvEntity.MapIEnumerable<CsvTheme,Theme>(csvThemes));
        if (csvCommands != null)
            context.Commands.AddRange(CsvEntity.MapIEnumerable<CsvCommand,Command>(csvCommands));
        if (csvCommandAttributes != null)
            context.Attributes.AddRange(CsvEntity.MapIEnumerable<CsvCommandAttribute,CommandAttribute>(csvCommandAttributes));
        if (csvReads != null)
            context.Reads.AddRange(CsvEntity.MapIEnumerable<CsvRead,Read>(csvReads));
        
        context.SaveChanges();
        return true;
    }
}