namespace DTO.Album;

using DTO.Track;

public class AlbumDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public long ArtistId { get; set; }
    public List<TrackDTO> Tracks { get; set; } = new();
}