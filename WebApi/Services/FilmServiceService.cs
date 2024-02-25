using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class FilmServiceService : IFilmService
{
    private readonly ICommentService _commentService;
    private readonly ApplicationContext _context;
    
    public FilmServiceService(ApplicationContext applicationContext, ICommentService commentService)
    {
        _context = applicationContext;
        _commentService = commentService;
    }
    
    public IEnumerable<FilmShortDataResponse> GetAllFilms()
    {
        var films = _context.Films.ToList();
        var filmsResponse = films.Select(film =>
            new FilmShortDataResponse(film.Id, film.Title, film.Genre, film.ImageUrl)).ToList();
        return filmsResponse;
    }

    public FilmResponse GetFilm(int id)
    {
        var film = _context.Films.First(film => film.Id == id);
        var comments = _commentService.GetCommentsByFilm(film)
            .Select(uc => new UserCommentResponse(uc.Id, uc.User.FirstName + " " + uc.User.LastName, uc.Comment));
        return new FilmResponse(film.Id, film.Title, film.Genre, film.Description, film.ImageUrl, comments);
    }
    
    public IEnumerable<FilmShortDataResponse> SearchFilms(string text)
    {
        text = text.ToLower();
        var films = _context.Films.Where(film => film.LowerTitle.ToLower().Contains(text));
        var filmsResponse = films.Select(film =>
            new FilmShortDataResponse(film.Id, film.Title, film.Genre, film.ImageUrl)).ToList();
        return filmsResponse;
    }
}