using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Attributes")]
public class CommandAttribute : Entity
{
    [Key] public Guid AttributeId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public string Description { get; set; }
    [Required] public Guid CommandId { get; set; }
    [JsonIgnore] public Command? Command { get; set; }
}