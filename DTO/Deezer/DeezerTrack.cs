namespace DTO.Deezer;

public class DeezerTrack
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public long Duration { get; set; }
    public long TrackPosition { get; set; }
    public string? Preview { get; set; }
    public DeezerAlbum? Album { get; set; }
    public DeezerArtist? Artist { get; set; }
}