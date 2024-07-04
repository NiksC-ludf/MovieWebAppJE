namespace MovieWebbApp.Tests
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using MovieWebApp.Controllers;
    using MovieWebApp.Models;
    using MovieWebApp.Services;

    public class MovieControllerTests
    {
        private readonly Mock<IOmdbService> _mockOmdbService;
        private readonly Mock<ILogger<MovieController>> _mockLogger;
        private readonly MovieController _controller;

        public MovieControllerTests()
        {
            _mockOmdbService = new Mock<IOmdbService>();
            _mockLogger = new Mock<ILogger<MovieController>>();
            _controller = new MovieController(_mockOmdbService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SearchMovies_ReturnsBadRequest_WhenTitleIsEmpty()
        {
            // Act
            var result = await _controller.SearchMovies("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task SearchMovies_ReturnsNotFound_WhenNoMoviesFound()
        {
            // Arrange
            _controller.ClearRecentSearches();
            _mockOmdbService.Setup(service => service.SearchMoviesAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<MovieSearchResults>());

            // Act
            var result = await _controller.SearchMovies("NonExistentMovie");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SearchMovies_ReturnsOk_WithMovies()
        {
            // Arrange
            _controller.ClearRecentSearches();
            var movies = new List<MovieSearchResults>
            {
                new MovieSearchResults { Title = "Test Movie", ImdbId = "tt1234567" }
            };
            _mockOmdbService.Setup(service => service.SearchMoviesAsync(It.IsAny<string>()))
                .ReturnsAsync(movies);

            // Act
            var result = await _controller.SearchMovies("Test Movie");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovies = Assert.IsType<List<MovieSearchResults>>(okResult.Value);
            Assert.Single(returnedMovies);
            Assert.Equal("Test Movie", returnedMovies[0].Title);
        }

        [Fact]
        public async Task SearchMovies_AddsToRecentSearches_WhenMovieFound()
        {
            // Arrange
            _controller.ClearRecentSearches();
            var movies = new List<MovieSearchResults>
            {
                new MovieSearchResults { Title = "Test Movie", ImdbId = "tt1234567" }
            };
            _mockOmdbService.Setup(service => service.SearchMoviesAsync(It.IsAny<string>()))
                .ReturnsAsync(movies);

            // Act
            await _controller.SearchMovies("Test Movie");
            var result = _controller.GetRecentSearches();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var recentSearches = Assert.IsType<List<string>>(okResult.Value);
            Assert.Single(recentSearches);
            Assert.Equal("Test Movie", recentSearches[0]);
        }

        [Fact]
        public async Task SearchMovies_ReturnsCode500_WhenExceptionThrown()
        {
            // Arrange
            _controller.ClearRecentSearches();
            _mockOmdbService.Setup(service => service.SearchMoviesAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.SearchMovies("Test Movie");

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, (result as StatusCodeResult).StatusCode);
        }

        [Fact]
        public async Task GetMovieDetails_ReturnsBadRequest_WhenImdbIdIsEmpty()
        {
            // Act
            var result = await _controller.GetMovieDetails("");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetMovieDetails_ReturnsNotFound_WhenNoDetailsFound()
        {
            // Arrange
            _mockOmdbService.Setup(service => service.GetMovieDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    new MovieDetails
                    {
                        Response = "False",
                    });

            // Act
            var result = await _controller.GetMovieDetails("NonExistentId");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetMovieDetails_ReturnsOk_WithMovieDetails()
        {
            // Arrange
            var movieDetails = new MovieDetails
            {
                Title = "Test Movie",
                imdbID = "tt1234567",
                Director = "Test Director"
            };
            _mockOmdbService.Setup(service => service.GetMovieDetailsAsync(It.IsAny<string>()))
                .ReturnsAsync(movieDetails);

            // Act
            var result = await _controller.GetMovieDetails("tt1234567");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMovieDetails = Assert.IsType<MovieDetails>(okResult.Value);
            Assert.Equal("Test Movie", returnedMovieDetails.Title);
            Assert.Equal("Test Director", returnedMovieDetails.Director);
        }

        [Fact]
        public async Task GetMovieDetails_ReturnsCode500_WhenExceptionThrown()
        {
            // Arrange
            _mockOmdbService.Setup(service => service.GetMovieDetailsAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await _controller.GetMovieDetails("tt1234567");

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, (result as StatusCodeResult).StatusCode);
        }
    }
}