using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Lib.DataBases;

public abstract class Entity
{
    #region Fields

    private DateTime _createdUTC { get; set; }

    private DateTime _updatedUTC { get; set; }

    #endregion

    #region Properties

    [Required] public bool IsActual { get; set; }

    [JsonIgnore]
    [DisplayName(nameof(CreatedUTC))]
    public virtual DateTime CreatedUTC
    {
        get => _createdUTC;
        set =>
            //if (value.Kind == DateTimeKind.Utc)
            _createdUTC = value;
        //else throw new InvalidDataException("Supports UTC kind only");
    }

    [JsonIgnore]
    [DisplayName(nameof(UpdatedUTC))]
    public DateTime UpdatedUTC
    {
        get => _updatedUTC;
        set =>
            //if (value.Kind == DateTimeKind.Utc)
            _updatedUTC = value;
        //else throw new InvalidDataException("Supports UTC kind only");
    }

    #endregion

    #region Methods

    public virtual void OnBeforeCreate()
    {
        CreatedUTC = DateTime.UtcNow;
        OnBeforeUpdate();
    }

    public virtual void OnBeforeUpdate()
    {
        UpdatedUTC = DateTime.UtcNow;
    }

    #endregion

    protected internal Entity()
    {
        OnBeforeCreate();
    }

    public string GetPostgresQuery()
    {
        var sb = new StringBuilder();

        var type = GetType();

        var properties = type.GetProperties()
            .Where(prop => prop.GetCustomAttribute(typeof(InvisibleItemAttribute)) == null).ToList();

        foreach (var propertyInfo in properties)
        {
            var value = propertyInfo?.GetValue(this);
            var propFullName = propertyInfo?.PropertyType.FullName;
            //#if DEBUG
            //                Console.WriteLine("Property: " + propertyInfo.Name + "\n\tType: " + (propFullName ?? ":?") + "\n\t\tValue:" + (value?.ToString() ?? "Null"));
            //#endif
            if ((propFullName == "System.String" || propFullName == "System.DateTime" ||
                 propFullName == "System.Guid" || propFullName.Contains("System.Nullable`1[[System.Guid")) &&
                value != null)
            {
                sb.Append("'");
                if (propFullName == "System.DateTime")
                    sb.Append(((DateTime)value).ToString("yyyy-MM-dd H:mm:ss.fffffff"));
                else
                    sb.Append(value?.ToString());

                sb.Append("'");
            }
            else
            {
                sb.Append(value?.ToString() ?? "null");
            }

            sb.Append(',');
        }

        sb.Remove(sb.Length - 1, 1);
        return sb.ToString();
    }
}