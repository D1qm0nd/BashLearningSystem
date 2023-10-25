using System.Linq;
using BashDataBase;
using DataModels;
using Lib.DataBases;
using NUnit.Framework;

namespace Tests.DataBase;

public class InsertEntityUnitTests
{
    private static IDataContext dataContext;

    [SetUp]
    public void InitTests()
    {
        dataContext = BashLearningDbContextFactory.CreateContext();
    }

    #region Tests for Post

    [Test]
    public void _1_AddAccount()
    {
        dataContext.GetEntity<Account>().Add(new Account()
        {
            Name = "TestName", 
            Surname = "TestSurname", 
            MiddleName = "TestMiddleName", 
            Login = "TestLogin",
            Password = "TestPassword"
        });
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _2_AddTheme()
    {
        dataContext.GetEntity<Theme>().Add(new Theme() { Name = "TestTheme2" });
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _3_AddCommand()
    {
        var themeId = dataContext.GetEntity<Theme>()
            .OrderBy(e => e.CreatedUTC).Last().ThemeId;
        dataContext.GetEntity<Command>().Add(new Command()
            { Text = "TestCommand", Description = "TestDescriptions", ThemeId = themeId });
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _4_AddCommandAttribute()
    {
        var commandId = dataContext.GetEntity<Command>()
            .OrderBy(e => e.CreatedUTC).Last().CommandId;
        dataContext.GetEntity<CommandAttribute>().Add(new CommandAttribute()
            { Text = "TestAttribute", Description = "TestDescription", CommandId = commandId });
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _5_AddExercise()
    {
        var theme = dataContext.GetEntity<Theme>()
            .OrderBy(t => t.CreatedUTC).Last();
        dataContext.GetEntity<Exercise>()
            .Add(new Exercise() { Name = "TestExercise", Text = "TestExercise" , ThemeId = theme.ThemeId});
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _6_AddQuest()
    {
        var exerciseId = dataContext.GetEntity<Exercise>()
            .OrderBy(e => e.CreatedUTC).Last().ExerciseId;
        var commandId = dataContext.GetEntity<Command>()
            .OrderBy(e => e.CreatedUTC).Last().CommandId;
        dataContext.GetEntity<Quest>().Add(new Quest()
            { ExerciseId = exerciseId, Text = "TestQuest", Answer = "TestAnswer", CommandId = commandId });
        dataContext.SaveRepositoryChanges();
    }

    [Test]
    public void _7_AddExerciseHistory()
    {
        var accountId = dataContext.GetEntity<Account>()
            .OrderBy(a => a.CreatedUTC).Last().AccountId;
        var exerciseId = dataContext.GetEntity<Exercise>()
            .OrderBy(e => e.CreatedUTC).Last().ExerciseId;
        dataContext.GetEntity<ExercisesHistory>().Add(new ExercisesHistory()
            { AccountId = accountId, ExerciseId = exerciseId, status = "In progress" });
        dataContext.SaveRepositoryChanges();
    }

    #endregion

    [Test]
    public void _8_GetThemes()
    {
    }

    [Test]
    public void _9_Add10Themes()
    {
        for (int i = 0; i < 100; i++)
        {
            _2_AddTheme();
            _3_AddCommand();
            _4_AddCommandAttribute();
            _5_AddExercise();
            _6_AddQuest();
            _7_AddExerciseHistory();
        }
    }

    [Test]
    public void _9_Add100CommandToLastTheme()
    {
        // _2_AddTheme();
        for (int i = 0; i < 100; i++)
        {
            _3_AddCommand();
            _4_AddCommandAttribute();
        }   
        // _5_AddExercise();
        // _6_AddQuest();
        // _7_AddExerciseHistory();
    }
}