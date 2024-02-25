using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTO.Request;

public record LoginRequest([EmailAddress] string Email, [MinLength(5)] string Password);