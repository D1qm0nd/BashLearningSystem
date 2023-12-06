using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;
using DataModels.Interfaces;
using Lib.DataBases;

namespace DataModels;

[Table("Commands")]
[Serializable]
public class Command : Entity, IBashCommand
{
    [Key] public Guid CommandId { get; set; }
    [Required] public string? Text { get; set; }
    [Required] public string? Description { get; set; }
    [Required] public Guid ThemeId { get; set; }

    [JsonIgnore] [InvisibleItem] public List<CommandAttribute> Attributes { get; set; }
    [JsonIgnore] [InvisibleItem] public Theme Theme { get; set; }

    public string GetFullDescription()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(GetDescription());
        stringBuilder.AppendLine(GetAttributesDescription());
        return stringBuilder.ToString();
    }

    public string GetAttributesDescription()
    {
        if (Attributes == null || Attributes?.FirstOrDefault() == null)
            return string.Empty;

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"Attributes:");
        foreach (var attribute in Attributes) stringBuilder.AppendLine($"\t{attribute.GetDescription()}");

        return stringBuilder.ToString();
    }

    public string GetDescription()
    {
        return Description;
    }
}