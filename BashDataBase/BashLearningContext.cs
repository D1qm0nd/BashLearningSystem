using DataModels;
using Lib.DataBases;
using Lib.DataBases.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BashDataBase;

public class BashLearningContext : BaseDataContext<BashLearningContext>, IDataContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Command> Commands { get; set; }
    public DbSet<CommandAttribute> CommandAttributes { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<ExercisesHistory> ExercisesHistory { get; set; }
    public DbSet<Quest> Quests { get; set; }
    public DbSet<Theme> Themes { get; set; }
    
    private string envConnectionStringPropertyName = "BashLearningDB";

    public BashLearningContext()
    {
    }
    
    public BashLearningContext(DbContextOptions<BashLearningContext> options) : base(options)
    {
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

    public void Migrate()
    {
        this.Database.Migrate();
    }

    public void Drop()
    {
        this.Database.EnsureDeleted();
    }

}

