namespace BlueSun.Services.Artists
{
    public interface IArtistService
    {
        public bool IsArtist(string userId);

        public int IdByUser(string userId);
    }
}
