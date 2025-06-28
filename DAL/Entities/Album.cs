namespace DAL.Entities;

public class Album
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public Artist Artist { get; set; }

    public List<Track> Tracks { get; set; } = new List<Track>();
}