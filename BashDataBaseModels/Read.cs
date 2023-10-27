using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Reads")]
public class Read : Entity
{
    [Key] public Guid ReadId { get; set; }
    [Required] public Guid ThemeId { get; set; }
    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public Theme? Theme { get; set; }
    [JsonIgnore] public User? User { get; set; }

}