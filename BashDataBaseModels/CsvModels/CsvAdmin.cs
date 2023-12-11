using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels.CSV;

[Table("Admins")]
public class CsvAdmin : CsvEntity
{
    public Guid AdminId { get; set; }
    public Guid UserId { get; set; }
}