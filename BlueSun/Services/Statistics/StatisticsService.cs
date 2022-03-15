namespace BlueSun.Services.Statistics
{
    using BlueSun.Data;

    public class StatisticsService : IStatisticsService
    {
        private readonly BlueSunDbContext data;

        public StatisticsService(BlueSunDbContext data) => this.data = data;

        public StatisticsServiceModel Total()
        {
            var totalNFTCollections = this.data.NFTCollections.Count();
            var totalNFTs = this.data.NFTs.Count();
            var totalUsers = this.data.Users.Count();

            return new StatisticsServiceModel
            {
                TotalNFTCollections = totalNFTCollections,
                TotalNFTs = totalNFTs,
                TotalUsers = totalUsers
            };
        }
    }
}
