﻿using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models.Entity;

[Table("users")]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    
    public int? RoleId { get; set; }
    public virtual Role Role { get; set; }
}