using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Questions")]
public class Question : Entity
{
    [Key] public Guid QuestionId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public Guid ThemeId { get; set; }
    [JsonIgnore] public Theme Theme { get; set; }
}