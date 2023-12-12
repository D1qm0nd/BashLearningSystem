using System.Collections.Generic;
using System.Linq;

namespace BashLearningModelsValidate;

public class ValidationResult
{
    public bool Result { get; private set; }
    public List<string>? Errors { get; private set; }
    
    public ValidationResult(List<string?> errors)
    {
        Errors = errors;
        Result = !Errors.Any();
    }
}