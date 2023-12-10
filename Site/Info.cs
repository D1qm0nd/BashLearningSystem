using System.Collections;
using System.Reflection;

namespace Site;

[Serializable]
public abstract class Info<TResult, TObject> where TResult: Info<TResult,TObject>
{
    public static IEnumerable<TResult> InfoList(IEnumerable<TObject> objects, Func<TObject,TResult> createInstance)
    {
        if (objects == null)
            yield break; 
        foreach (var obj in objects)
        {
            yield return createInstance.Invoke(obj);
        }
    }

    private static IEnumerable<PropertyInfo> GetIEnumerableProperties(Type type)
    {
        var props = type.GetProperties()
            .Where(prop => prop.PropertyType.IsGenericType);
        return props;
    }
}