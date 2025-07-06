using Microsoft.AspNetCore.Mvc;
using BL;
using DTO.Artist;
using Tools;
using Microsoft.Extensions.Logging;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArtistController : ControllerBase
{
    private readonly ArtistManager _artistManager;
    private readonly IDeezerService _deezerService;
    private readonly ILogger<ArtistController> _logger;

    public ArtistController(
        ArtistManager artistManager,
        IDeezerService deezerService,
        ILogger<ArtistController> logger)
    {
        _artistManager = artistManager;
        _deezerService = deezerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all artists in local collection
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ArtistDTO>), 200)]
    public IActionResult GetAll()
    {
        var artists = _artistManager.Artists.ToList();

        return Ok(artists);
    }

    /// <summary>
    /// Get artist by Deezer ID (checks local collection first, then Deezer API)
    /// </summary>
    /// <param name="id">Deezer artist ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ArtistDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(int id)
    {
        var existingArtist = _artistManager.Artists.FirstOrDefault(a => a.DeezerId == id);
        if (existingArtist != null)
        {
            return Ok(existingArtist);
        }

        try
        {
            var deezerArtist = await _deezerService.GetArtist(id);
            return Ok(deezerArtist);
        }
        catch
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Add artist to local collection by Deezer ID
    /// </summary>
    /// <param name="id">Deezer artist ID</param>
    // [HttpPost("{id}")]
    // [ProducesResponseType(typeof(ArtistDTO), 201)]
    // [ProducesResponseType(404)]
    // public async Task<IActionResult> Add(int id)
    // {
    //     try
    //     {
    //         var deezerArtist = await _deezerService.GetArtist(id);

    //         _artistManager.Add(deezerArtist);
    //         return CreatedAtAction(nameof(GetById), new { id = deezerArtist.DeezerId }, deezerArtist);
    //     }
    //     catch
    //     {
    //         return NotFound();
    //     }
    // }

    /// <summary>
    /// Remove artist from local collection
    /// </summary>
    /// <param name="id">Deezer artist ID</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public IActionResult Remove(int id)
    {
        var artist = _artistManager.Artists.FirstOrDefault(a => a.DeezerId == id);
        if (artist == null) return NotFound();

        _artistManager.Remove(artist);
        return NoContent();
    }
}