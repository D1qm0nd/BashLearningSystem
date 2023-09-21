using Lib.DataBases;

namespace WebApp.LearningSystem.BussinesModels;

public interface IHaveDataContext
{
    public IDataContext DataContext { get; set; }
}