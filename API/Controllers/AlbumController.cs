using Microsoft.AspNetCore.Mvc;
using BL;
using DTO.Album;
using Tools;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlbumController : ControllerBase
{
    private readonly ArtistManager _artistManager;
    private readonly IDeezerService _deezerService;
    private readonly ILogger<AlbumController> _logger;

    public AlbumController(
        ArtistManager artistManager,
        IDeezerService deezerService,
        ILogger<AlbumController> logger)
    {
        _artistManager = artistManager;
        _deezerService = deezerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all albums in local collection
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<AlbumDTO>), 200)]
    public IActionResult GetAll()
    {
        var albums = _artistManager.Artists.ToList();

        return Ok(albums);
    }

    /// <summary>
    /// Get album by ID (checks local collection first, then Deezer API for tracks)
    /// </summary>
    /// <param name="id">Album ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AlbumDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var album = _artistManager.Artists
            .SelectMany(a => a.Albums)
            .FirstOrDefault(a => a.Id == id);

        if (album != null)
        {
            return Ok(album);
        }

        return NotFound();
    }

    /// <summary>
    /// Remove album from local collection
    /// </summary>
    /// <param name="id">Album ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Remove(int id)
    {
        var album = _artistManager.Artists
            .SelectMany(a => a.Albums)
            .FirstOrDefault(a => a.Id == id);

        if (album == null) return NotFound();

        album.Artist.Albums.Remove(album);
        return NoContent();
    }
}