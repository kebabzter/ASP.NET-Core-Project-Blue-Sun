namespace BlueSun.Services.Artists
{
    public interface IArtistService
    {
        public bool IsArtist(string userId);

        public int IdByUser(string userId);

        public string UserById(int id);

        public void AddArtist(string name, string phoneNumber, string userId);
    }
}
