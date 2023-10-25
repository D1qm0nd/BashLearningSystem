using BashDataBase;
using Lib.DataBases;

namespace db_init
{
    public class Program
    {
        public static void Main()
        {
            DbContextFactory<BashLearningContext>.CreateContext().Migrate();
        }
    }
}