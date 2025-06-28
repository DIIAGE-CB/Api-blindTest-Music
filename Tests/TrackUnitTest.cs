using DAL.Entities;
using FluentAssertions;
using BL;

namespace Tests
{
    public class TrackUnitTest
    {
        [Fact]
        public void TestTrackOk()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";


            Track track = new Track();
            track.Id = 3135553;
            track.Title = "One More Time";
            track.Album = album;
            track.Duration = 320;
            track.TrackPosition = 2;
            track.Preview = "https://cdns-preview-b.dzcdn.net/stream/c-b2e0166bba75a78251d6dca9c9c3b41a-7.mp3";


            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            manager.Artists.Should().NotBeEmpty().And.HaveCount(1);

            manager.AddTrack(artist.DeezerId, album.Id, track);

            Artist a = manager.Artists.Find(a => a.DeezerId == 27);

            a.Should().NotBeNull();

            a.Albums.Should().NotBeEmpty().And.HaveCount(1);

            a.Albums.First().Tracks.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public void TestTrackKoArtistException()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";


            Track track = new Track();
            track.Id = 3135553;
            track.Title = "One More Time";
            track.Album = album;
            track.Duration = 320;
            track.TrackPosition = 2;
            track.Preview = "https://cdns-preview-b.dzcdn.net/stream/c-b2e0166bba75a78251d6dca9c9c3b41a-7.mp3";


            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            Action addAction = () => { manager.AddTrack(123456789, album.Id, track); };

            addAction.Should().Throw<Exception>().WithMessage("Artist does not exist");
        }

        [Fact]
        public void TestTrackKoAlbumException()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";


            Track track = new Track();
            track.Id = 3135553;
            track.Title = "One More Time";
            track.Album = album;
            track.Duration = 320;
            track.TrackPosition = 2;
            track.Preview = "https://cdns-preview-b.dzcdn.net/stream/c-b2e0166bba75a78251d6dca9c9c3b41a-7.mp3";


            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            Action addAction = () => { manager.AddTrack(artist.DeezerId, 123456789, track); };

            addAction.Should().Throw<Exception>().WithMessage("Album does not exist");
        }

        [Fact]
        public void TestTrackKoTrackException()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";

            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            Action addAction = () => { manager.AddTrack(artist.DeezerId, album.Id, null); };

            addAction.Should().Throw<ArgumentNullException>().WithMessage("Track is null*");
        }

        [Fact]
        public void TestRemoveTrackOk()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";

            Track t1 = new Track();
            t1.Id = 3135553;
            t1.Title = "One More Time";
            t1.Album = album;
            t1.Duration = 320;
            t1.TrackPosition = 2;
            t1.Preview = "https://cdns-preview-e.dzcdn.net/stream/c-e77d23e0c8ed7567a507a6d1b6a9ca1b-9.mp3";


            Track t2 = new Track();
            t2.Id = 3135554;
            t2.Title = "Aerodynamic";
            t2.Album = album;
            t2.Duration = 212;
            t2.TrackPosition = 3;
            t2.Preview = "https://cdns-preview-b.dzcdn.net/stream/c-b2e0166bba75a78251d6dca9c9c3b41a-7.mp3";



            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            manager.AddTrack(artist.DeezerId, album.Id, t1);
            manager.AddTrack(artist.DeezerId, album.Id, t2);

            var result = manager.RemoveTrack(artist.DeezerId, album.Id, t1);

            var a = manager.Artists.Find(ar => ar.DeezerId == artist.DeezerId);

            a.Albums.Find(al => al.Id == album.Id).Tracks.Should().NotBeEmpty().And.HaveCount(1);
        }

        [Fact]
        public void TestRemoveTrackKo()
        {
            Artist artist = new Artist();

            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            Album album = new Album();
            album.Id = 3155975;
            album.Title = "Human After All";
            album.Cover = "http://e-cdn-images.dzcdn.net/images/cover/39e2281a0e9f564e73b4f49dfa06f4ab/500x500-000000-80-0-0.jpg";

            Track track = new Track();
            track.Id = 3135553;
            track.Title = "One More Time";
            track.Album = album;
            track.Duration = 320;
            track.TrackPosition = 2;
            track.Preview = "https://cdns-preview-e.dzcdn.net/stream/c-e77d23e0c8ed7567a507a6d1b6a9ca1b-9.mp3";


            ArtistManager manager = new ArtistManager();

            manager.Add(artist);

            manager.AddAlbum(artist.DeezerId, album);

            manager.AddTrack(artist.DeezerId, album.Id, track);

            Action removeAction = () => { manager.RemoveTrack(artist.DeezerId, album.Id, null); };

            removeAction.Should().Throw<ArgumentNullException>().WithMessage("Track is null*");

            removeAction = () => { manager.RemoveTrack(artist.DeezerId, 123456789, track); };
            removeAction.Should().Throw<ArgumentNullException>().WithMessage("Album does not exist*");

            removeAction = () => { manager.RemoveTrack(123, album.Id, track); };
            removeAction.Should().Throw<ArgumentNullException>().WithMessage("Artist does not exist*");


            manager.RemoveTrack(artist.DeezerId, album.Id, track);
            removeAction = () => { manager.RemoveTrack(artist.DeezerId, album.Id, track); };

            removeAction.Should().Throw<Exception>().WithMessage("Track does not exist in the album");

        }
    }
}