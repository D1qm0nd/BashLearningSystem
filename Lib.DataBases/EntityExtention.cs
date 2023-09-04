using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;

namespace Lib.DataBases
{
    public static class EntityExtention
    {
        public static string CreateBulkInsertQuery(this IEnumerable<Entity> entities)
        {
            var first = entities.FirstOrDefault();
            if (first == null) return "";

            var table = first.GetType().GetCustomAttribute<TableAttribute>()?.Name ?? (first.GetType().Name + "s");

            var sb = new StringBuilder();

            sb.Append("INSERT INTO " + "\"" + table + "\" (" + GetBaseFieldNamesToCSV(first.GetBaseFieldNames()) + ") VALUES ");

            foreach (var entity in entities)
            {
                sb.Append("(" + entity.GetPostgresQuery() + "),");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append(";");
            return sb.ToString();
        }

        public static string GetBaseFieldNamesToCSV(this IEnumerable<string> list)
        {
            var sb = new StringBuilder();
            foreach (var name in list)
            {
                sb.Append(name+" ,");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public static List<string> GetBaseFieldNames(this Entity obj)
        {
            var type = obj.GetType();

            var properties = type.GetProperties();

            List<string> fields = new List<string>();

            foreach (var property in properties)
            {
                var invAttr = property.GetCustomAttribute<InvisibleItemAttribute>();
                if (invAttr != null)
                    continue;

                //var foreignKeyAttr = property.GetCustomAttribute<ForeignKeyAttribute>();
                //if (foreignKeyAttr != null)
                //{
                //    fields.Add("\"" + foreignKeyAttr.Name + "\"");
                //}
                //else 
                    fields.Add("\"" + property.Name + "\"");
            }
            return fields;
        }

    }
}
