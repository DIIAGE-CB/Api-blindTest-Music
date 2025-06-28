namespace DTO.Deezer;

public class DeezerCollectionResponse<T>
{
    public List<T> Data { get; set; } = new();
    public int Total { get; set; }
    public string? Next { get; set; }
}