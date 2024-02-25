using WebApi.Models.DTO.Request;
using WebApi.Models.DTO.Response;
using WebApi.Models.Entity;

namespace WebApi.Services.Interfaces;

public interface ICommentService
{
    void AddComment(AddCommentRequest addCommentRequest, User user);
    IEnumerable<UserComment> GetCommentsByFilm(Film film);
    IEnumerable<CommentResponse> GetAllComments();
    void DeleteComment(int id);
}