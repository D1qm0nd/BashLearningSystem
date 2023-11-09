using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Admins")]
public class Admin : Entity
{
    [Key] public Guid AdminId { get; set; }
    [Required] public Guid UserId { get; set; }
    [JsonIgnore] public User User { get; set; }
}