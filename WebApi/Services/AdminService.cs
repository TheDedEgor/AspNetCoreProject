using WebApi.Models;
using WebApi.Models.DTO.Response;
using WebApi.Models.Entity;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationContext _context;
    
    public AdminService(ApplicationContext applicationContext)
    {
        _context = applicationContext;
    }
    
    public void AddFilm(FilmResponse filmResponse)
    {
        var newFilm = new Film
        {
            Title = filmResponse.Title,
            Genre = filmResponse.Genre,
            ImageUrl = filmResponse.ImageUrl,
            Description = filmResponse.Description,
            LowerTitle = filmResponse.Title.ToLower()
        };
        _context.Films.Add(newFilm);
        _context.SaveChanges();
    }
    
    public void UpdateFilm(FilmResponse filmResponse)
    {
        var film = new Film
        {
            Id = filmResponse.Id,
            Title = filmResponse.Title,
            Genre = filmResponse.Genre,
            ImageUrl = filmResponse.ImageUrl,
            Description = filmResponse.Description,
            LowerTitle = filmResponse.Title.ToLower()
        };
        _context.Films.Update(film);
        _context.SaveChanges();
    }
    
    public void DeleteComment(int id)
    {
        var userComment = _context.UserComments.First(uc => uc.Id == id);
        _context.UserComments.Remove(userComment);
        _context.SaveChanges();
    }
    
    public void DeleteFilm(int id)
    {
        var film = _context.Films.First(uc => uc.Id == id);
        var userComments = _context.UserComments.Where(uc => uc.Film == film).ToList();
        _context.UserComments.RemoveRange(userComments);
        _context.Films.Remove(film);
        _context.SaveChanges();
    }
    
    public IEnumerable<CommentResponse> GetAllComments()
    {
        return _context.UserComments
            .Select(uc => new CommentResponse(uc.Id, uc.Film.Title, uc.User.FirstName + " " + uc.User.LastName, uc.Comment))
            .ToList();
    }
    
    public IEnumerable<FilmResponse> GetAllFilms()
    {
        var films = _context.Films.ToList();
        return films
            .Select(film => new  FilmResponse(film.Id, film.Title, film.Genre, film.Description, film.ImageUrl, new List<UserCommentResponse>()))
            .ToList();
    }
}