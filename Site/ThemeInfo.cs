using BashDataBaseModels;

namespace Site;

[Serializable]
public class ThemeInfo : Info<ThemeInfo, Theme>
{
    public Guid ID { get; set; }
    public string Name { get; set; }
    public DateTime UpdatedUTC { get; set; }
    public List<CommandInfo> Commands { get; set; }

    public ThemeInfo(Theme theme)
    {
        ID = theme.ThemeId;
        Name = theme.Name;
        UpdatedUTC = theme.UpdatedUTC;
        Commands = CommandInfo.InfoList(theme.Commands, (command) => new CommandInfo(command)).ToList();
    }
}