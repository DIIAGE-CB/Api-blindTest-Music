using DTO.Album;

namespace DTO.Artist
{
    public class ArtistDTO
    {
        public long DeezerId { get; set; }
        public string Name { get; set; }
        public string PictureSmall { get; set; }
        public string PictureMedium { get; set; }
        public string PictureBig { get; set; }
        public string PictureXl { get; set; }

        // Simplified albums without circular reference
        public List<AlbumShortDTO> Albums { get; set; } = new();
    }
}