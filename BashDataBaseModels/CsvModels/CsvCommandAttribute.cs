using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels.CSV;

[Table("Attributes")]
public class CsvCommandAttribute : CsvEntity
{
    public Guid AttributeId { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public Guid CommandId { get; set; }
}