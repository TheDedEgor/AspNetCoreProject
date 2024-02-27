using WebApi.Models;
using WebApi.Models.DTO.Request;
using WebApi.Models.Entity;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationContext _context;
    private readonly PasswordService _passwordService;
    
    public AuthService(ApplicationContext applicationContext, PasswordService passwordService)
    {
        _context = applicationContext;
        _passwordService = passwordService;
    }
    
    public Account Login(LoginRequest loginRequest)
    {
        var email = loginRequest.Email;
        var password = loginRequest.Password;
        
        var passwordHash = _passwordService.GetPasswordHash(password);
        var account = _context.Accounts.FirstOrDefault(ac => ac.Email == email && ac.Password == passwordHash);
        if (account == null)
        {
            throw new BadHttpRequestException("Неверный логин или пароль");
        }

        return account;
    }

    public Account Register(RegisterRequest registerRequest)
    {
        var email = registerRequest.Email;
        var password = registerRequest.Password;
        
        var account = _context.Accounts.FirstOrDefault(ac => ac.Email == email);
        if (account != null)
        {
            throw new BadHttpRequestException("Пользователь с таким email существует");
        }
        
        var passwordHash = _passwordService.GetPasswordHash(password);
        var newUser = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Role = _context.Roles.First(role => role.Name == "user")
        };
        _context.Users.Add(newUser);
        var newAccount = new Account {Email = email, Password = passwordHash, User = newUser};
        _context.Accounts.Add(newAccount);
        _context.SaveChanges();
        
        return newAccount;
    }
}