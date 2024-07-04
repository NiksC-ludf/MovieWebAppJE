namespace OmdbServiceTests
{
    using System.Net;
    using Moq;
    using Moq.Protected;
    using MovieWebApp.Services;
    using MovieWebApp.Models;
    using Newtonsoft.Json;

    public class OmdbServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly HttpClient _client;
        private readonly OmdbService _omdbService;
        private readonly string _apiKey = "your_api_key";

        public OmdbServiceTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _client = new HttpClient(_mockHandler.Object)
            {
                BaseAddress = new Uri("http://www.omdbapi.com/")
            };
            _omdbService = new OmdbService(_client, _apiKey);
        }

        [Fact]
        public async Task SearchMoviesAsync_ReturnsResults_OnValidQuery()
        {
            // Arrange
            var returnJson = JsonConvert.SerializeObject(new OmdbSearchResponse
            {
                Search = new List<MovieSearchResults>
            {
                new MovieSearchResults { Title = "Test Movie", ImdbId = "tt123456" }
            }
            });

            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(returnJson),
                });

            // Act
            var result = await _omdbService.SearchMoviesAsync("Test Movie");

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Movie", result[0].Title);
            Assert.Equal("tt123456", result[0].ImdbId);
        }

        [Fact]
        public async Task SearchMoviesAsync_ThrowsException_OnHttpRequestException()
        {
            // Arrange
            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _omdbService.SearchMoviesAsync("Test Movie"));
        }

        [Fact]
        public async Task SearchMoviesAsync_ReturnsNull_WhenApiResponseIsUnexpectedFormat()
        {
            // Arrange
            var invalidJson = "{ \"unexpected\": \"format\" }";

            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(invalidJson),
                });

            // Act
            var result = await _omdbService.SearchMoviesAsync("Test Movie");

            // Assert
            Assert.Null(result);
        }



        [Fact]
        public async Task GetMovieDetailsAsync_ReturnsDetails_OnValidImdbId()
        {
            // Arrange
            var returnJson = JsonConvert.SerializeObject(new MovieDetails
            {
                Title = "Test Movie",
                imdbID = "tt123456",
                Director = "Test Director",
                Response = "True",
            });

            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(returnJson),
                });

            // Act
            var result = await _omdbService.GetMovieDetailsAsync("tt123456");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal("Test Director", result.Director);
        }

        [Fact]
        public async Task GetMovieDetailsAsync_ThrowsException_OnHttpRequestException()
        {
            // Arrange
            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _omdbService.GetMovieDetailsAsync("tt123456"));
        }

        [Fact]
        public async Task GetMovieDetailsAsync_ReturnsNull_WhenApiResponseIsUnexpectedFormat()
        {
            // Arrange
            var invalidJson = "{ \"unexpected\": \"format\" }";

            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(invalidJson),
                });

            // Act
            var result = await _omdbService.GetMovieDetailsAsync("tt123456");

            // Assert
            Assert.Equal("False", result.Response);
        }

    }
}