using BlueSun.Services.Statistics;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueSun.Test.Mocks
{
    public static class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.Total())
                    .Returns(new StatisticsServiceModel
                    {
                        TotalNFTCollections = 5,
                        TotalUsers = 10,
                        TotalNFTs = 15
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}
