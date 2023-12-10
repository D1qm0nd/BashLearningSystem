using BashDataBaseModels;

namespace Site;

[Serializable]
public class AttributeInfo : Info<AttributeInfo, CommandAttribute>
{
    public Guid ID { get; set; }
    public Guid CommandID { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }

    public AttributeInfo(CommandAttribute attribute)
    {
        ID = attribute.AttributeId;
        Text = attribute.Text;
        Description = attribute.Description;
        CommandID = attribute.CommandId;
    }
}