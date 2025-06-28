namespace DTO.Track;

public class TrackShortDTO
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public long Duration { get; set; }
    public long TrackPosition { get; set; }
    public string Preview { get; set; } = string.Empty;
}