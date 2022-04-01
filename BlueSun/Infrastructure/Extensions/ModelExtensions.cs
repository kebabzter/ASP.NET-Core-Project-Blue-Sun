namespace BlueSun.Infrastructure.Extensions
{
    using BlueSun.Services.NFTCollections.Models;

    public static class ModelExtensions
    {
        public static string GetInformation(this INFTCollectionModel collection)
            => collection.Name;
    }
}
