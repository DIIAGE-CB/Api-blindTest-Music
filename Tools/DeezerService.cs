using Microsoft.Extensions.Logging;
using DTO.Album;
using DTO.Artist;
using DTO.Track;
using DTO.Search;
using DTO.Deezer;
using static Tools.TimedOperation.TimedOperation;

namespace Tools;

public interface IDeezerService
{
    Task<ArtistDTO> GetArtist(int id);
    Task<List<AlbumShortDTO>> GetArtistAlbums(int artistId, int limit = 100);
    Task<List<TrackShortDTO>> GetAlbumTracks(int albumId);
    Task<SearchResultDTO> Search(string query, int limit = 5);
    Task<IEnumerable<TrackShortDTO>> GetArtistTracks(int artistId, int limit = 100);
}

/*
 * Implementation of IDeezerService providing access to Deezer public API.
 * Responsible for making REST calls using HttpClient, deserializing JSON responses into
 * DTOs, and providing a typed interface for client code.
 */
public class DeezerService : IDeezerService
{
    // important: HttpJsonClient is a wrapper around HttpClient that handles JSON serialization/deserialization
    private readonly HttpJsonClient _httpJsonClient;
    private readonly ILogger<DeezerService> _logger;
    private const string BaseUrl = "https://api.deezer.com";

    public DeezerService(HttpClient httpClient, ILogger<DeezerService> logger)
    {
        _httpJsonClient = new HttpJsonClient(httpClient);
        _logger = logger;
    }

    private Task<T> ExecuteTimedCallAsync<T>(
        string operationName,
        Func<Task<T>> func,
        Action<T, TimeSpan>? resultLogger = null,
        Action<Exception, TimeSpan>? errorHandler = null)
    {
        return TimedCallAsync(
            _logger,
            operationName,
            func,
            resultLogger,
            errorHandler);
    }

    public async Task<SearchResultDTO> Search(string query, int limit = 5)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new SearchResultDTO { Tracks = new List<TrackSearchResultDTO>() };

        return await ExecuteTimedCallAsync("DeezerSearch", async () =>
        {
            var url = $"{BaseUrl}/search?q={Uri.EscapeDataString(query)}&limit={limit}";
            var result = await _httpJsonClient.GetAndDeserializeAsync<DeezerCollectionResponse<DeezerTrack>>(url);

            var tracks = result?.Data?.Select(t => new TrackSearchResultDTO
            {
                Id = t.Id,
                Title = t.Title ?? "Unknown Track",
                Duration = t.Duration,
                Preview = t.Preview ?? string.Empty,
                Album = t.Album != null ? new AlbumShortDTO
                {
                    Id = t.Album.Id,
                    Title = t.Album.Title ?? "Unknown Album",
                    Cover = t.Album.CoverSmall ?? string.Empty
                } : new AlbumShortDTO(),
                Artist = t.Artist != null ? new ArtistShortDTO
                {
                    DeezerId = t.Artist.Id,
                    Name = t.Artist.Name ?? "Unknown Artist",
                    PictureSmall = t.Artist.PictureSmall ?? string.Empty
                } : new ArtistShortDTO()
            }).Take(limit).ToList() ?? new List<TrackSearchResultDTO>();

            return new SearchResultDTO { Tracks = tracks };
        },
        resultLogger: (result, duration) =>
            _logger.LogDebug("Found {Count} tracks in {Ms}ms", result.Tracks.Count, duration.TotalMilliseconds));
    }

    public async Task<ArtistDTO> GetArtist(int id)
    {
        return await ExecuteTimedCallAsync("GetDeezerArtist", async () =>
        {
            var deezerArtist = await _httpJsonClient.GetAndDeserializeAsync<DeezerArtist>($"{BaseUrl}/artist/{id}")
                               ?? throw new InvalidOperationException("Artist response was null");

            var albums = await GetArtistAlbums(id);

            return new ArtistDTO
            {
                DeezerId = deezerArtist.Id,
                Name = deezerArtist.Name ?? "Unknown Artist",
                PictureSmall = deezerArtist.PictureSmall ?? string.Empty,
                PictureMedium = deezerArtist.PictureMedium ?? string.Empty,
                PictureBig = deezerArtist.PictureBig ?? string.Empty,
                PictureXl = deezerArtist.PictureXl ?? string.Empty,
                Albums = albums
            };
        },
        errorHandler: (ex, duration) =>
            _logger.LogWarning("Failed to get artist after {Ms}ms: {Error}", duration.TotalMilliseconds, ex.Message));
    }

    public Task<List<AlbumShortDTO>> GetArtistAlbums(int artistId, int limit = 100) =>
        ExecuteTimedCallAsync(
            "GetDeezerArtistAlbums",
            async () =>
            {
                var url = $"{BaseUrl}/artist/{artistId}/albums?limit={limit}";
                var result = await _httpJsonClient.GetAndDeserializeAsync<DeezerCollectionResponse<DeezerAlbum>>(url);

                return result?.Data?.Select(a => new AlbumShortDTO
                {
                    Id = a.Id,
                    Title = a.Title ?? "Unknown Album",
                    Cover = a.CoverMedium ?? string.Empty
                }).ToList() ?? new List<AlbumShortDTO>();
            },
            resultLogger: (albums, duration) =>
                _logger.LogDebug("Fetched {Count} albums in {Ms}ms", albums.Count, duration.TotalMilliseconds));

    public Task<List<TrackShortDTO>> GetAlbumTracks(int albumId) =>
        ExecuteTimedCallAsync(
            "GetDeezerAlbumTracks",
            async () =>
            {
                var url = $"{BaseUrl}/album/{albumId}/tracks";
                var result = await _httpJsonClient.GetAndDeserializeAsync<DeezerCollectionResponse<DeezerTrack>>(url);

                return result?.Data?.Select(t => new TrackShortDTO
                {
                    Id = t.Id,
                    Title = t.Title ?? "Unknown Track",
                    Duration = t.Duration,
                    TrackPosition = t.TrackPosition,
                    Preview = t.Preview ?? string.Empty
                }).ToList() ?? new List<TrackShortDTO>();
            },
            resultLogger: (tracks, duration) =>
                _logger.LogDebug("Fetched {Count} tracks in {Ms}ms", tracks.Count, duration.TotalMilliseconds));

    public Task<IEnumerable<TrackShortDTO>> GetArtistTracks(int artistId, int limit = 100) =>
        ExecuteTimedCallAsync(
            "GetDeezerArtistTracks",
            async () =>
            {
                var url = $"{BaseUrl}/artist/{artistId}/top?limit={limit}";
                var result = await _httpJsonClient.GetAndDeserializeAsync<DeezerCollectionResponse<DeezerTrack>>(url);

                return result?.Data?.Select(t => new TrackShortDTO
                {
                    Id = t.Id,
                    Title = t.Title ?? "Unknown Track",
                    Duration = t.Duration,
                    TrackPosition = t.TrackPosition,
                    Preview = t.Preview ?? string.Empty
                }) ?? Enumerable.Empty<TrackShortDTO>();
            },
            resultLogger: (tracks, duration) =>
                _logger.LogDebug("Fetched {Count} top tracks in {Ms}ms", tracks.Count(), duration.TotalMilliseconds));
}

