using WebApi.Models;
using WebApi.Models.Entity;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class UserServiceService : IUserService
{
    private readonly ApplicationContext _context;

    public UserServiceService(ApplicationContext applicationContext)
    {
        _context = applicationContext;
    }

    public User GetUser(string email)
    {
        var account = _context.Accounts.First(ac => ac.Email == email);
        return account.User;
    }
}