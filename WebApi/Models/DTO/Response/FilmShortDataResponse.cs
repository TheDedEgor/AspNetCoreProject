namespace WebApi.Models.DTO.Response;

public record FilmShortDataResponse(
    int Id,
    string Title,
    string Genre,
    string ImageUrl
);