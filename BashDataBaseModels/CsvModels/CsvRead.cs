using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels.CSV;

[Table("Reads")]
public class CsvRead : CsvEntity
{
    public Guid ReadId { get; set; }
    public Guid ThemeId { get; set; }
    public Guid UserId { get; set; }
}