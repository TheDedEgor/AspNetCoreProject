using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entity;

[Table("user_comments")]
public class UserComment
{
    public int Id { get; set; }
    
    public string Comment { get; set; }
    
    public int? FilmId { get; set; }
    public virtual Film Film { get; set; }
    
    public int? UserId { get; set; }
    public virtual User User { get; set; }
}