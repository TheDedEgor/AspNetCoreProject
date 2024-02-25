using WebApi.Models.Entity;

namespace WebApi.Services.Interfaces;

public interface IUserService
{
    User GetUser(string email);
}