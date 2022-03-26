namespace BlueSun.Test.Services
{
    using BlueSun.Data.Models;
    using BlueSun.Services.Artists;
    using BlueSun.Test.Mocks;
    using Xunit;

    public class ArtistServiceTest
    {
        private const string UserId = "TestUserId";

        [Fact]
        public void IsArtistShouldReturnTrueWhenUserIsArtist()
        {
            //Arrange
            var artistService = GetArtistService();

            //Act
            var result = artistService.IsArtist(UserId);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void IsArtistShouldReturnFalseWhenUserIsNotArtist()
        {
            //Arrange
            var artistService = GetArtistService();

            //Act
            var result = artistService.IsArtist("AnotherUserId");

            //Assert
            Assert.False(result);
        }

        private static IArtistService GetArtistService()
        {
            var data = DatabaseMock.Instance;

            data.Artists.Add(new Artist { UserId = UserId });
            data.SaveChanges();

            return new ArtistService(data);
        }
    }
}
