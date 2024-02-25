using WebApi.Models.DTO.Response;

namespace WebApi.Services.Interfaces;

public interface IFilmService
{
    IEnumerable<FilmShortDataResponse> SearchFilms(string text);
    IEnumerable<FilmShortDataResponse> GetAllFilms();
    FilmResponse GetFilm(int id);
}