namespace BlueSun.Services.NFTCollections.Models
{
    public class LatestNFTCollectionServiceModel : INFTCollectionModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }
    }
}
