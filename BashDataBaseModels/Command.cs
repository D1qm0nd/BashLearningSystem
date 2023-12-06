using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Commands")]
public class Command : Entity
{
    [Key] public Guid CommandId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public string Description { get; set; }
    [JsonIgnore] public List<CommandAttribute> Attributes { get; set; }

    [Required] public Guid ThemeId { get; set; }
    [JsonIgnore] public Theme Theme { get; set; }
}