using DataModels;
using WebClasses;

namespace LearningSite.WebApp.BusinessModels;

public class LearningSession : Session<Account>
{
    public Guid? CurrentThemeId { get; private set; }

    public void SetCurrentThemeId(Guid id)
    {
        CurrentThemeId = id;
    }

    public void SetCurrentThemeIdDefault()
    {
        CurrentThemeId = null;
    }

    public LearningSession()
    {
        Id = Guid.NewGuid();
    }
}