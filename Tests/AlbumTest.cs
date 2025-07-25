using FluentAssertions;
using DAL.Entities;
using BL;

namespace Tests
{
    public class AlbumUnitTest
    {
        [Fact]
        public void TestAlbumOk()
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

            manager.Artists.Should().NotBeEmpty().And.HaveCount(1);

            manager.Artists.Find( a => a.DeezerId == 27)?.Albums.Should().NotBeEmpty().And.HaveCount(1);

            album.Artist.Should().NotBeNull();

            album.Artist.Should().Be(artist);
        }


        
        [Fact]

        public void TestAlbumListAddKo()
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

           

            Album nullAlbum = null;

            Action action = () => manager.AddAlbum(artist.DeezerId, nullAlbum);

            action.Should().Throw<ArgumentNullException>();


            Action addAction = () => manager.AddAlbum(9999999, album);

            addAction.Should().Throw<Exception>().WithMessage("Artist is not in the list");


         



        }

        /*

        [Fact]

        public void TestArtistListRemoveOk()
        {

            ArtistManager artistManager = new ArtistManager();
            Artist artist = new Artist();
            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            artistManager.Add(artist);

            Artist johnny = new Artist();
            johnny.DeezerId = 1060;
            johnny.Name = "Johnny Hallyday";
            johnny.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/56x56-000000-80-0-0.jpg";
            johnny.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/250x250-000000-80-0-0.jpg";
            johnny.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/500x500-000000-80-0-0.jpg";
            johnny.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/1000x1000-000000-80-0-0.jpg";

            artistManager.Add(johnny);


            artistManager.Artists.Should().NotBeEmpty().And.HaveCount(2);
            Artist nullArtist = null;

            Action action = () => artistManager.Remove(nullArtist);
            action.Should().Throw<ArgumentNullException>();

            Artist notInTheListArtist = new Artist();
            notInTheListArtist.DeezerId = 5555;

            Action removeActionKO = () => artistManager.Remove(notInTheListArtist);

            removeActionKO.Should().Throw<Exception>().WithMessage("Artist is not in the list");


            artistManager.Remove(johnny);

            artistManager.Artists.Should().NotBeEmpty().And.HaveCount(1);






        }


        [Fact]

        public void TestArtistListUpdateOk()
        {

            ArtistManager artistManager = new ArtistManager();
            Artist artist = new Artist();
            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            artistManager.Add(artist);

            Artist johnny = new Artist();
            johnny.DeezerId = 1060;
            johnny.Name = "Johnny Hallyday";
            johnny.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/56x56-000000-80-0-0.jpg";
            johnny.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/250x250-000000-80-0-0.jpg";
            johnny.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/500x500-000000-80-0-0.jpg";
            johnny.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/1000x1000-000000-80-0-0.jpg";

            artistManager.Add(johnny);


            artistManager.Artists.Should().NotBeEmpty().And.HaveCount(2);
            Artist nullArtist = null;

            Action action = () => artistManager.Update(nullArtist);
            action.Should().Throw<ArgumentNullException>();

            Artist notInTheListArtist = new Artist();
            notInTheListArtist.DeezerId = 5555;

            Action updateActionKO = () => artistManager.Update(notInTheListArtist);

            updateActionKO.Should().Throw<Exception>().WithMessage("Artist is not in the list");


            Artist updatedJohnny = new Artist();
            updatedJohnny.DeezerId = 1060;
            updatedJohnny.Name = "Johnny Hallyday The best EVER ";

            artistManager.Update(updatedJohnny);


            artistManager.Artists.Find(a => a.DeezerId == 1060).Name.Should().Be(updatedJohnny.Name);






        }

        [Fact]

        public void TestArtistListDisplayNamesOk()
        {

            ArtistManager artistManager = new ArtistManager();
            Artist artist = new Artist();
            artist.DeezerId = 27;
            artist.Name = "Daft Punk";
            artist.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/56x56-000000-80-0-0.jpg";
            artist.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/250x250-000000-80-0-0.jpg";
            artist.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/500x500-000000-80-0-0.jpg";
            artist.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/f2bc007e9133c946ac3c3907ddc5d2ea/1000x1000-000000-80-0-0.jpg";


            artistManager.Add(artist);

            Artist johnny = new Artist();
            johnny.DeezerId = 1060;
            johnny.Name = "Johnny Hallyday";
            johnny.PictureSmall = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/56x56-000000-80-0-0.jpg";
            johnny.PictureMedium = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/250x250-000000-80-0-0.jpg";
            johnny.PictureBig = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/500x500-000000-80-0-0.jpg";
            johnny.PictureXl = "https://e-cdns-images.dzcdn.net/images/artist/a8cbf6cc9d2808237b23b1159b56afba/1000x1000-000000-80-0-0.jpg";

            artistManager.Add(johnny);


            artistManager.Artists.Should().NotBeEmpty().And.HaveCount(2);



            Assert.True(artistManager.Artists[0].Name == artist.Name);

            Assert.True(artistManager.Artists[1].Name == johnny.Name);




        }
        */
    }
}