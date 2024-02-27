using WebApi.Models.DTO.Response;

namespace WebApi.Services.Interfaces;

public interface IAdminService
{
    IEnumerable<CommentResponse> GetAllComments();
    void DeleteComment(int id);
    void DeleteFilm(int id);
    IEnumerable<FilmResponse> GetAllFilms();
    void AddFilm(FilmResponse filmResponse);
    void UpdateFilm(FilmResponse filmResponse);
}