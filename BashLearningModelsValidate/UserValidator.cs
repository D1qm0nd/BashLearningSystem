using System.Collections.Generic;
using BashDataBaseModels;

namespace BashLearningModelsValidate;

public class UserValidator : Validator<User>
{
    public override ValidationResult Validate(User obj)
    {
        List<string> errors = new();
        if (obj.UserId == null)
            errors.Add($"{nameof(obj.UserId)} is invalid");
        if (obj.Login.Length < 8 && obj.Login.Length > 64)
            errors.Add($"{nameof(obj.Login)} is invalid");
        if (obj.Password.Length < 8 && obj.Login.Length > 255)
            errors.Add($"{nameof(obj.Password)} is invalid");
        return new ValidationResult(errors);
    }
}