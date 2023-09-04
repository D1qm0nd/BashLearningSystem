using DataModels;
using Lib.DataBases;
using Lib.DataBases.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BashDataBase;

public class BashLearningContext : BaseDataContext<BashLearningContext>, IRepository
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Command> Commands { get; set; }
    public DbSet<CommandAttribute> CommandAttributes { get; set; }
    
    private static string envConnectionStringPropertyName;

    public BashLearningContext()
    {
        
    }
    
    public BashLearningContext(DbContextOptions<BashLearningContext> options) : base(options)
    {
        envConnectionStringPropertyName = "BashLearningDB"; 
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        var connectionString = Environment.GetEnvironmentVariable(envConnectionStringPropertyName);

        if (connectionString == null)
            throw new ConnectionStringExistingException();

        try
        {
            optionsBuilder.UseNpgsql(connectionString);
        }
        catch (Exception ex)
        {
            throw new IncorrectConnectionStringException();
        }

    }

    public int SaveRepositoryChanges()
    {
        this.SaveChanges();
        return 0;
    }
}

