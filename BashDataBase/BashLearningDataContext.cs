using Lib.DataBases;

namespace BashDataBase;

public class BashLearningDataContext : IDataContext
{
    public IRepository Repository { get; }

    public BashLearningDataContext()
    {
        Repository = new BashLearningContext();
    }
    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}