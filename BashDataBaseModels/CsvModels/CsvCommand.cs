using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels.CSV;

[Table("Commands")]
public class CsvCommand : CsvEntity
{
    public Guid CommandId { get; set; }
    public string Text { get; set; }
    public string Description { get; set; }
    public Guid ThemeId { get; set; }
}