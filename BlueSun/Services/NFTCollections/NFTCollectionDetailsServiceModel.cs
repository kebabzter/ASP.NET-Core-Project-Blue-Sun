namespace BlueSun.Services.NFTCollections
{
    public class NFTCollectionDetailsServiceModel : NFTCollectionServiceModel
    {
        public string Description { get; init; }

        public int ArtistId { get; init; }

        public string ArtistName { get; init; }

        public int CategoryId { get; init; }

        public string UserId { get; init; }
    }
}
