using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTO.Request;

public record RegisterRequest(
    [EmailAddress]
    string Email, 
    [MinLength(5)]
    string Password, 
    string FirstName, 
    string LastName
);