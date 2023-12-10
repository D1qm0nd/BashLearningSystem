using System.Net.Mime;
using BashDataBaseModels;

namespace Site;

[Serializable]
public class CommandInfo : Info<CommandInfo,Command>
{
    public Guid ID { get; set; }
    public Guid ThemeID { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }

    public List<AttributeInfo> Attributes { get; set; }

    public CommandInfo(Command command)
    {
        ID = command.CommandId;
        ThemeID = command.ThemeId;
        Text = command.Text;
        Description = command.Description;
        Attributes = AttributeInfo.InfoList(command.Attributes, (attribute) => new AttributeInfo(attribute)).ToList();
    }
}