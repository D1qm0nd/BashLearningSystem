using BashDataBaseModels;
using Lib.DataBases;
using Lib.DataBases.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BashLearningDB;

public class BashLearningContext : DbContext, IDataContext
{
    private string envConnectionStringPropertyName = "BashLearningDB";

    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<Command> Commands { get; set; }
    public DbSet<CommandAttribute> Attributes { get; set; }
    public DbSet<Read> Reads { get; set; }
    
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
}