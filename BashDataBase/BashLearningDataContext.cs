using DataModels;
using Lib.DataBases;
using Microsoft.EntityFrameworkCore;

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