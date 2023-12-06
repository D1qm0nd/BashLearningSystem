using Lib.DataBases;

namespace BashLearningModelsValidate;

public abstract class Validator<T> where T : Entity
{
    public virtual ValidationResult Validate(T obj)
    {
        throw new NotImplementedException();
    }
}