namespace DAL.Entities
{
    public class Artist
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int DeezerId { get; set; }
        public string Name { get; set; }
        public string PictureSmall { get; set; }
        public string PictureMedium { get; set; }
        public string PictureBig { get; set; }
        public string PictureXl { get; set; }

        public List<Album> Albums { get; set; } = new List<Album>();
    }
}
