using DTO.Album;
using DTO.Artist;

namespace DTO.Search;

public class TrackSearchResultDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long Duration { get; set; }
    public string Preview { get; set; }
    public AlbumShortDTO Album { get; set; }
    public ArtistShortDTO Artist { get; set; }
}