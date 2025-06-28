using System.Text.Json.Serialization;

namespace DTO.Deezer;

public class DeezerArtist
{
    public long Id { get; set; }
    public string? Name { get; set; }
    [JsonPropertyName("picture_small")]
    public string? PictureSmall { get; set; }
    [JsonPropertyName("picture_medium")]
    public string? PictureMedium { get; set; }
    [JsonPropertyName("picture_big")]
    public string? PictureBig { get; set; }
    [JsonPropertyName("picture_xl")]
    public string? PictureXl { get; set; }
}