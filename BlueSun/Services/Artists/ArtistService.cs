namespace BlueSun.Services.Artists
{
    using BlueSun.Data;

    public class ArtistService : IArtistService
    {
        private readonly BlueSunDbContext data;

        public ArtistService(BlueSunDbContext data) 
            => this.data = data;

        public bool IsArtist(string userId)
         => this.data
            .Artists
            .Any(a => a.UserId == userId);

        public int IdByUser(string userId) 
            => this.data
                .Artists
                .Where(a => a.UserId == userId)
                .Select(a => a.Id)
                .FirstOrDefault();

        public string UserById(int id)
        => this.data
            .Artists
            .Where(a => a.Id == id)
            .Select(a => a.UserId)
            .FirstOrDefault();
    }
}
