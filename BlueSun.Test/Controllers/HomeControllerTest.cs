namespace BlueSun.Test.Controller
{
    using AutoMapper;
    using BlueSun.Controllers;
    using BlueSun.Test.Mocks;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;
    public class HomeControllerTest
    {
        [Fact]
        public void ErrorShouldReturnView()
        {
            // Arrange
            var homeController = new HomeController(null,null, Mock.Of<IMapper>());

            // Act
            var result = homeController.Error();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
