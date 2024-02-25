using WebApi.Models.DTO.Request;
using WebApi.Models.Entity;

namespace WebApi.Services.Interfaces;

public interface IAuthService
{
    Account Login(LoginRequest loginRequest);
    Account Register(RegisterRequest registerRequest);
}