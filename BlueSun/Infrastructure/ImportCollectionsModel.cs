namespace BlueSun.Infrastructure
{

    public class ImportCollectionsModel
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public bool IsPublic { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public int ArtistId { get; init; }
    }
}
