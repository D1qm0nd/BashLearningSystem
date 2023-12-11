namespace Lib.DataBases;

public class CsvEntity : Entity
{
    public T Map<T>(T entity) where T : Entity
    {
        var thisType = this.GetType();
        var entType = entity.GetType();
        var props = thisType.GetProperties();

        foreach (var propertyInfo in props)
        {
            var currentProp = propertyInfo;
            var entProp = entType.GetProperties()
                .FirstOrDefault(p => p.PropertyType == currentProp.PropertyType && p.Name == currentProp.Name);
            if (entProp != null)
            {
                var value = currentProp.GetValue(this);
                if (value.GetType() == typeof(DateTime))
                    value = ((DateTime)value).ToUniversalTime();
                entProp.SetValue(entity, value);
            }
        }

        return entity;
    }

    public static IEnumerable<TResult> MapIEnumerable<TCsv, TResult>(IEnumerable<TCsv> enumerable)
        where TCsv : CsvEntity
        where TResult : Entity, new()
    {
        foreach (var csv in enumerable)
        {
            yield return csv.Map(new TResult());
        }
    }
}