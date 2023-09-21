using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using Lib.DataBases;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataModels;

[Table("Accounts"), Serializable]
public class Account : Entity, IValidatableObject
{
    [Key] public Guid AccountId { get; set; }
    [Required] public string? Login { get; set; }
    [Required] public string? Password { get; set; }
    [Required] public string? Surname { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? MiddleName { get; set; }
    public byte[]? Image { get; set; }
    
    // [JsonIgnore, InvisibleItem] public List<Exercise> Exercises { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var errors = new List<ValidationResult>();
        if (Login == null)
            errors.Add(new ValidationResult($"{nameof(Login)} is incorrect"));
        if (Password == null)
            errors.Add(new ValidationResult($"{nameof(Password)} is incorrect"));
        if (Surname == null)
            errors.Add(new ValidationResult($"{nameof(Surname)} is incorrect"));
        if (Name == null)
            errors.Add(new ValidationResult($"{nameof(Name)} is incorrect"));
        if (MiddleName == null)
            errors.Add(new ValidationResult($"{nameof(MiddleName)} is incorrect"));
        return errors;
    }

    #region May be later

    //TODO:  Придумать апгрейд валидации)
//     public IEnumerable<ValidationResult> Validate2(ValidationContext validationContext)
//     {
//         var propsValues = this.GetProperties();
//         var errors = new List<ValidationResult>();
//         foreach (var prop in propsValues)
//         {
//             errors.PropertyValidate(propsValues, prop.Key);
//         }
//
//         return errors;
//     }
// }
//
// public static class ValidateExtentions
// {
//     public static List<ValidationResult> PropertyValidate(this List<ValidationResult> errors,
//         IEnumerable<KeyValuePair<string, object?>> properties, string propertyName)
//     {
//         foreach (var prop in properties)
//         {
//             if (propertyName == prop.Key)
//             {
//                 if (prop.Value == null)
//                 {
//                     errors.Add(new ValidationResult($"{prop.Key} is incorrect"));
//                 }
//             }
//         }
//
//         return errors;
//     }
//
//     public static IEnumerable<KeyValuePair<string, object?>> GetProperties(this object obj)
//     {
//         var type = obj.GetType();
//         var properties = type.GetProperties();
//         foreach (var prop in properties)
//         {
//             yield return new KeyValuePair<string, object?>(prop.Name, prop.GetValue(obj));
//         }
//     }

    #endregion
}