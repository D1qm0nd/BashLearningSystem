using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Lib.DataBases;

namespace BashDataBaseModels;

[Table("Users")]
public class User : Entity
{
    [Key] public Guid UserId { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    [Required] public string Middlename { get; set; }
    
    [JsonIgnore] public List<Admin> AdminRecords { get; set; }

    public string FullName() => $"{Surname} {Name} {Middlename}";
}