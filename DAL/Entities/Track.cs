namespace DAL.Entities;

public class Track
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Album Album { get; set; }
    public int Duration { get; set; }
    public int TrackPosition { get; set; }
    public string Preview { get; set; }
}