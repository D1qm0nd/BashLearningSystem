using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace DataModels;

[Table("Exercises")]
[Serializable]
public class Exercise : Entity
{
    [Key] public Guid ExerciseId { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? Text { get; set; }

    [Required] public Guid ThemeId { get; set; }

    [JsonIgnore] [InvisibleItem] public Theme Theme { get; set; }

    [JsonIgnore] [InvisibleItem] public List<Quest> Questions { get; set; }
}