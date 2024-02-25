using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entity;

[Table("accounts")]
public class Account
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    
    public int? UserId { get; set; }
    public virtual User User { get; set; }
}