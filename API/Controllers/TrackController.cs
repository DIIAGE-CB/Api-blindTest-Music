using Microsoft.AspNetCore.Mvc;
using BL;
using DTO.Track;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TrackController : ControllerBase
{
    private readonly ArtistManager _artistManager;
    private readonly ILogger<TrackController> _logger;

    public TrackController(
        ArtistManager artistManager,
        ILogger<TrackController> logger)
    {
        _artistManager = artistManager;
        _logger = logger;
    }

    /// <summary>
    /// Get all tracks in local collection
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<TrackDTO>), 200)]
    public IActionResult GetAll()
    {
        var tracks = _artistManager.Artists
            .SelectMany(a => a.Albums)
            .SelectMany(a => a.Tracks)
            .ToList();

        return Ok(tracks);
    }

    /// <summary>
    /// Get track by ID from local collection
    /// </summary>
    /// <param name="id">Track ID</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TrackDTO), 200)]
    [ProducesResponseType(404)]
    public IActionResult GetById(int id)
    {
        var track = _artistManager.Artists
            .SelectMany(a => a.Albums)
            .SelectMany(a => a.Tracks)
            .FirstOrDefault(t => t.Id == id);

        if (track == null) return NotFound();

        return Ok(track);
    }

    /// <summary>
    /// Remove track from local collection
    /// </summary>
    /// <param name="artistId">Artist Deezer ID</param>
    /// <param name="albumId">Album ID</param>
    /// <param name="trackId">Track ID</param>
    // [HttpDelete("{artistId}/{albumId}/{trackId}")]
    // [ProducesResponseType(204)]
    // [ProducesResponseType(404)]
    // public IActionResult Remove(int artistId, int albumId, int trackId)
    // {
    //     var track = new TrackDTO { Id = trackId };
    //     var removedTrack = _artistManager.RemoveTrack(artistId, albumId, track);

    //     if (removedTrack == null) return NotFound();

    //     return NoContent();
    // }
}