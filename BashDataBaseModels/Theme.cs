using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Serializable]
[Table("Themes")]
public class Theme : Entity
{
    [Key] public Guid ThemeId { get; set; }
    [Required] public string Name { get; set; }
    [JsonIgnore] public List<Command> Commands { get; set; }
    [JsonIgnore] public List<Question> Questions { get; set; }
    [JsonIgnore] public List<Read> Reads { get; set; }
    
}