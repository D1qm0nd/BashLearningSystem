using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels.CSV;

[Serializable]
[Table("Themes")]
public class CsvTheme : CsvEntity
{
    public Guid ThemeId { get; set; }
    public string Name { get; set; }
}