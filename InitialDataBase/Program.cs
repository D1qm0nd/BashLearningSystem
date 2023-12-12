using BashLearningDB;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace db_init
{
    public class Program
    {
        public static void Main()
        {
            var action = Environment.GetEnvironmentVariable("ACTION");
            if (action == null)
                throw new EnvironmentVariableExistingException("ACTION: INIT/DROP");
            
            var context = new BashLearningContext();
            switch (action)
            {
                case "INIT": 
                    context.Database.Migrate();
                    break;
                case "DROP":
                    context.Database.EnsureDeleted();
                    break;
            }
            // DbContextFactory<BashLearningContext>.CreateContext().Migrate();
        }
    }
}