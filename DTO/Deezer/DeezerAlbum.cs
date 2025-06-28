using System.Text.Json.Serialization;

namespace DTO.Deezer;

public class DeezerAlbum
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Cover { get; set; }
    [JsonPropertyName("cover_small")]
    public string? CoverSmall { get; set; }
    [JsonPropertyName("cover_medium")]
    public string? CoverMedium { get; set; }
    [JsonPropertyName("cover_big")]
    public string? CoverBig { get; set; }
    [JsonPropertyName("cover_xl")]
    public string? CoverXl { get; set; }
}