using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Themes")]
public class Theme : Entity
{
    [Key] public Guid ThemeId { get; set; }
    [Required] public string Name { get; set; }
    [JsonIgnore] public List<Command> Commands { get; set; }
}