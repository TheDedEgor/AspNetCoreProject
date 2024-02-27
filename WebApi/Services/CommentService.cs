using WebApi.Models;
using WebApi.Models.DTO.Request;
using WebApi.Models.DTO.Response;
using WebApi.Models.Entity;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class CommentService : ICommentService
{
    private readonly ApplicationContext _context;
    
    public CommentService(ApplicationContext applicationContext)
    {
        _context = applicationContext;
    }

    public void AddComment(AddCommentRequest addCommentRequest, User user)
    {
        var userComment = new UserComment
        {
            Comment = addCommentRequest.Comment,
            FilmId = addCommentRequest.FilmId,
            User = user
        };
        _context.UserComments.Add(userComment);
        _context.SaveChanges();
    }
    
    public IEnumerable<UserComment> GetCommentsByFilm(Film film)
    {
        return _context.UserComments.Where(uc => uc.Film == film).ToList();
    }
}