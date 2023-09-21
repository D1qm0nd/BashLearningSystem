using Lib.DataBases;

namespace WebApp.LearningSystem.BussinesModels;

public interface IHaveAccount<T> where T : Entity
{
    public T? Account { get; set; }

    public bool isAuthorized
    {
        get
        {
            if (Account != null)
            {
                return true;
            }

            return false;
        }
    }

    public string AccountToString { get; }
}