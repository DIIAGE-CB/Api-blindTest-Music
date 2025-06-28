using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Tools;
using DTO.Search;
using DTO.Album;
using DTO.Track;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ResponseCache(Duration = 3600)]
public class SearchController : ControllerBase
{
    private readonly IDeezerService _deezerService;
    private readonly ILogger<SearchController> _logger;

    public SearchController(
        IDeezerService deezerService,
        ILogger<SearchController> logger)
    {
        _deezerService = deezerService;
        _logger = logger;
    }

    /// <summary>
    /// Search for tracks, albums, and artists on Deezer
    /// </summary>
    /// <param name="query">Search query string</param>
    /// <param name="limit">Maximum number of results to return (1-50)</param>
    /// <returns>Search results containing tracks</returns>
    /// <response code="200">Returns the search results</response>
    /// <response code="400">If the query is empty or invalid</response>
    /// <response code="404">If no results were found for the query</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(SearchResultDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SearchResultDTO>> Search(
        [FromQuery, Required] string query,
        [FromQuery, Range(1, 50)] int limit = 5)
    {
        try
        {
            _logger.LogInformation("Search initiated for query: {Query}", query);

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest();
            }

            var results = await _deezerService.Search(query, limit);

            if (results == null || results.Tracks.Count == 0)
            {
                _logger.LogWarning("No results found for query: {Query}", query);
                return NotFound();
            }

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Search failed for query: {Query}", query);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get albums for a specific artist
    /// </summary>
    /// <param name="artistId">Deezer artist ID</param>
    /// <returns>List of albums for the artist</returns>
    /// <response code="200">Returns the artist's albums</response>
    /// <response code="400">If the artist ID is invalid</response>
    /// <response code="404">If no albums were found for the artist</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("album")]
    [ProducesResponseType(typeof(List<AlbumShortDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<AlbumShortDTO>>> ArtistsAlbumSearch(
        [FromQuery, Range(1, int.MaxValue)] int artistId)
    {
        try
        {
            _logger.LogInformation("Fetching albums for artist ID: {ArtistId}", artistId);

            var results = await _deezerService.GetArtistAlbums(artistId);

            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch albums for artist ID: {ArtistId}", artistId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get top tracks for a specific artist
    /// </summary>
    /// <param name="artistId">Deezer artist ID</param>
    /// <param name="limit">Maximum number of tracks to return (1-200)</param>
    /// <returns>List of top tracks for the artist</returns>
    /// <response code="200">Returns the artist's top tracks</response>
    /// <response code="400">If the artist ID or limit is invalid</response>
    /// <response code="404">If no tracks were found for the artist</response>
    /// <response code="500">If there was an internal server error</response>
    [HttpGet("track")]
    [ProducesResponseType(typeof(List<TrackShortDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TrackShortDTO>>> ArtistsTrackSearch(
        [FromQuery, Range(1, int.MaxValue)] int artistId,
        [FromQuery, Range(1, 200)] int limit = 100)
    {
        try
        {
            _logger.LogInformation("Fetching top tracks for artist ID: {ArtistId}", artistId);

            var results = await _deezerService.GetArtistTracks(artistId, limit);

            if (results == null || results.Count() == 0)
            {
                _logger.LogWarning("No top tracks found for artist ID: {ArtistId}", artistId);
                return NotFound();
            }

            return Ok(results);
        }
        catch
        {
            _logger.LogError("Erreur lors de la récupération des top tracks pour l'artiste ID: {ArtistId}", artistId);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}