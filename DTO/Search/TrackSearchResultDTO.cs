namespace DTO.Search;

public class TrackSearchResultDTO
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long Duration { get; set; }
    public string Preview { get; set; }
    public AlbumSearchResultDTO Album { get; set; }
    public ArtistSearchResultDTO Artist { get; set; }
}