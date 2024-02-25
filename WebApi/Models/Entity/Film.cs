using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entity;

[Table("films")]
public class Film
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    
    public string LowerTitle { get; set; }
}