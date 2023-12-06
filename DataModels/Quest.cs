using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace DataModels;

public class Quest : Entity
{
    [Key] public Guid QuestionId { get; set; }
    [Required] public string? Text { get; set; }
    [Required] public string? Answer { get; set; }
    [Required] public Guid ExerciseId { get; set; }
    [Required] public Guid CommandId { get; set; }

    [JsonIgnore] [InvisibleItem] public Command Command { get; set; }
    [JsonIgnore] [InvisibleItem] public Exercise Exercise { get; set; }
}