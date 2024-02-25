using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entity;

[Table("roles")]
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
}