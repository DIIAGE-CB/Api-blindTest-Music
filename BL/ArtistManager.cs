using DAL.Entities;

namespace BL;

/// <summary>
/// The <c>ArtistManager</c> class is part of the Business Logic (BL) layer of the application.
/// It manages a collection of <see cref="Artist"/> objects and provides operations for adding,
/// updating, and removing artists, as well as managing their albums and tracks.
/// </summary>
public class ArtistManager
{
    public List<Artist> Artists { get; } = new List<Artist>();

    /// <summary>
    /// Adds a new artist to the collection.
    /// </summary>
    /// <param name="artist">The <see cref="Artist"/> object to add. It must not be null, and its
    /// <c>DeezerId</c> must be unique in the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="artist"/> is null.</exception>
    /// <exception cref="Exception">Thrown when an artist with the same <c>DeezerId</c> already exists.</exception>
    public void Add(Artist artist)
    {
        if (artist == null) throw new ArgumentNullException(nameof(artist));
        if (Artists.Any(a => a.DeezerId == artist.DeezerId))
            throw new Exception("Artist is already in the list");
        Artists.Add(artist);
    }

    /// <summary>
    /// Removes an existing artist from the collection.
    /// </summary>
    /// <param name="artist">The <see cref="Artist"/> object to remove.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="artist"/> is null.</exception>
    /// <exception cref="Exception">Thrown when the artist is not found in the collection.</exception>
    public void Remove(Artist artist)
    {
        if (artist == null) throw new ArgumentNullException(nameof(artist));
        var existing = Artists.FirstOrDefault(a => a.DeezerId == artist.DeezerId);
        if (existing == null) throw new Exception("Artist is not in the list");
        Artists.Remove(existing);
    }

    /// <summary>
    /// Updates the properties of an existing artist.
    /// </summary>
    /// <param name="artist">The updated <see cref="Artist"/> object containing new information.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="artist"/> is null.</exception>
    /// <exception cref="Exception">Thrown when the artist is not found in the collection.</exception>
    public void Update(Artist artist)
    {
        if (artist == null) throw new ArgumentNullException(nameof(artist));
        var existing = Artists.FirstOrDefault(a => a.DeezerId == artist.DeezerId);
        if (existing == null) throw new Exception("Artist is not in the list");

        existing.Name = artist.Name;
        existing.PictureSmall = artist.PictureSmall;
        existing.PictureMedium = artist.PictureMedium;
        existing.PictureBig = artist.PictureBig;
        existing.PictureXl = artist.PictureXl;
    }

    /// <summary>
    /// Adds an album to a specific artist.
    /// </summary>
    /// <param name="deezerId">The Deezer ID of the artist to associate the album with.</param>
    /// <param name="album">The <see cref="Album"/> object to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="album"/> is null.</exception>
    /// <exception cref="Exception">Thrown when the artist is not found.</exception>
    public void AddAlbum(int deezerId, Album album)
    {
        if (album == null) throw new ArgumentNullException(nameof(album));
        var artist = Artists.FirstOrDefault(a => a.DeezerId == deezerId)
                     ?? throw new Exception("Artist is not in the list");

        album.Artist = artist;
        artist.Albums.Add(album);
    }

    /// <summary>
    /// Adds a track to a specific album of an artist.
    /// </summary>
    /// <param name="deezerId">The Deezer ID of the artist.</param>
    /// <param name="albumId">The ID of the album to which the track should be added.</param>
    /// <param name="track">The <see cref="Track"/> object to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="track"/> is null.</exception>
    /// <exception cref="Exception">Thrown when the artist or album is not found.</exception>
    public void AddTrack(int deezerId, int albumId, Track track)
    {
        if (track == null) throw new ArgumentNullException(nameof(track), "Track is null");

        var artist = Artists.FirstOrDefault(a => a.DeezerId == deezerId)
                     ?? throw new Exception("Artist does not exist");

        var album = artist.Albums.FirstOrDefault(al => al.Id == albumId)
                    ?? throw new Exception("Album does not exist");

        track.Album = album;
        album.Tracks.Add(track);
    }

    /// <summary>
    /// Removes a track from a specific album.
    /// </summary>
    /// <param name="deezerId">The Deezer ID of the artist.</param>
    /// <param name="albumId">The ID of the album containing the track.</param>
    /// <param name="track">The <see cref="Track"/> object to remove.</param>
    /// <returns>The removed <see cref="Track"/> object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="track"/> is null,
    /// or when the artist or album does not exist.</exception>
    /// <exception cref="Exception">Thrown when the track is not found in the album.</exception>
    public Track RemoveTrack(int deezerId, int albumId, Track track)
    {
        if (track == null) throw new ArgumentNullException(nameof(track), "Track is null");

        var artist = Artists.FirstOrDefault(a => a.DeezerId == deezerId)
                     ?? throw new ArgumentNullException("artist", "Artist does not exist");

        var album = artist.Albums.FirstOrDefault(al => al.Id == albumId)
                    ?? throw new ArgumentNullException("album", "Album does not exist");

        var existingTrack = album.Tracks.FirstOrDefault(t => t.Id == track.Id);
        if (existingTrack == null) throw new Exception("Track does not exist in the album");

        album.Tracks.Remove(existingTrack);
        return existingTrack;
    }
}
