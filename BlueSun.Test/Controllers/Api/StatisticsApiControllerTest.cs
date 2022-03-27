namespace BlueSun.Test.Controllers.Api
{
    using BlueSun.Controllers.Api;
    using BlueSun.Test.Mocks;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class StatisticsApiControllerTest
    {
        [Fact]
        public void GetStatisticsShouldReturnTotalStatistics()
        {
            // Arrange
            var statisticsController = new StatisticsApiController(StatisticsServiceMock.Instance);

            // Act
            var result = statisticsController.GetStatistics();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(5, result.TotalNFTCollections);
            Assert.Equal(10, result.TotalUsers);
            Assert.Equal(15, result.TotalNFTs);
        } 
    }
}
