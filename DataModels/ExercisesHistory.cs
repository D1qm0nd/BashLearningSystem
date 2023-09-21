using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace DataModels;

[Table("History"), Serializable]
public class ExercisesHistory : Entity
{
    [Key] public Guid HistoryId { get; set; }

    [Required] public Guid AccountId { get; set; }
    [Required] public Guid ExerciseId { get; set; }

    /// <summary>
    /// не проходил, проходит, прошёл
    /// </summary>
    [Required]
    public string status { get; set; }

    [JsonIgnore, InvisibleItem] public Account Account { get; set; }
    [JsonIgnore, InvisibleItem] public Exercise Exercise { get; set; }

}