using BashDataBase;
using LearningSite.WebApp.BusinessModels;
using Lib.DataBases;

namespace LearningSite.WebApp.Models;

public class ThemeViewModel
{
    public IDataContext Context { get; set; }
    public LearningSession Session { get; set; }
    
    public ThemeViewModel(LearningSession session, IDataContext context)
    {
        Session = session;
        Context = context;
    }

}