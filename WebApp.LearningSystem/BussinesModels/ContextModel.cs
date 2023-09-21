using Lib.DataBases;

namespace WebApp.LearningSystem.BussinesModels;

public class ContextModel : IHaveDataContext
{
    public IDataContext DataContext { get; set; }
    
    public ContextModel(IDataContext context)
    {
        DataContext = context;
    }
}