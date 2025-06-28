using DTO.Album;

namespace DTO.Track;

public class TrackDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long Duration { get; set; }
    public long TrackPosition { get; set; }
    public string Preview { get; set; }

    // Reference to album without circular reference
    public AlbumShortDTO Album { get; set; }
}