using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTO.Response;
using WebApi.Services.Interfaces;

namespace WebApi.Controllers;

[ApiController]
[Route("api/films")]
public class FilmController : ControllerBase
{
    private readonly IFilmService _filmServiceService;
    
    public FilmController(IFilmService filmServiceService)
    {
        _filmServiceService = filmServiceService;
    }
    
    [HttpGet]
    public IEnumerable<FilmShortDataResponse> GetAllFilmsShortData()
    { 
        return _filmServiceService.GetAllFilmsShortData();
    }
    
    [HttpGet("{id}")]
    public FilmResponse GetFilmWithComments(int id)
    {
        return _filmServiceService.GetFilmWithComments(id);
    }
    
    [HttpGet("search")]
    public IEnumerable<FilmShortDataResponse> SearchFilms(string text)
    { 
        return _filmServiceService.SearchFilms(text);
    }
}