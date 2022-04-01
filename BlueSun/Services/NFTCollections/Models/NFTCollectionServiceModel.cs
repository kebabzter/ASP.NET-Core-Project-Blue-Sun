namespace BlueSun.Services.NFTCollections.Models
{
    public class NFTCollectionServiceModel : INFTCollectionModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string CategoryName { get; init; }

        public string ImageUrl { get; init; }

        public bool IsPublic { get; init; }
    }
}
