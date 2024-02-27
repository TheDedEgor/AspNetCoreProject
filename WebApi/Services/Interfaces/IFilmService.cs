using WebApi.Models.DTO.Response;

namespace WebApi.Services.Interfaces;

public interface IFilmService
{
    IEnumerable<FilmShortDataResponse> SearchFilms(string text);
    IEnumerable<FilmShortDataResponse> GetAllFilmsShortData();
    FilmResponse GetFilmWithComments(int id);
}