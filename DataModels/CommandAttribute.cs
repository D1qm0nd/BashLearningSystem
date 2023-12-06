using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DataModels.Interfaces;
using Lib.DataBases;

namespace DataModels;

[Table("Attributes")]
[Serializable]
public class CommandAttribute : Entity, IBashCommandAttribute
{
    #region Properies

    [Key] public Guid AttributeId { get; set; }
    [Required] public string? Text { get; set; }
    [Required] public string? Description { get; set; }
    [Required] public Guid CommandId { get; set; }

    #region Navigate properties

    [JsonIgnore] [InvisibleItem] public Command Command { get; set; }

    #endregion

    #endregion

    #region Methods

    public string GetDescription()
    {
        return Description;
    }

    #endregion
}