using BashDataBase;
using LearningSite.WebApp.BusinessModels;
using Lib.DataBases;

namespace LearningSite.WebApp.Models;

public class HomeViewModel
{
    public LearningSession Session { get; private set; }
    // public IDataContext Context { get; private set; }
    public HomeViewModel(LearningSession session)
    {
        Session = session;
        // Context = context;
    }
}