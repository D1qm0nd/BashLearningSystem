using BashLearningDB;
using Lib.DataBases;

namespace db_init
{
    public class Program
    {
        public static void Main()
        {
            // new BashLearningContext().Drop();
            new BashLearningContext().Migrate();
            // DbContextFactory<BashLearningContext>.CreateContext().Migrate();
        }
    }
}