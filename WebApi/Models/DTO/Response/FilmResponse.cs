using WebApi.Models.Entity;

namespace WebApi.Models.DTO.Response;

public record FilmResponse(
    int Id,
    string Title,
    string Genre,
    string Description,
    string ImageUrl,
    IEnumerable<UserCommentResponse> Comments
);
