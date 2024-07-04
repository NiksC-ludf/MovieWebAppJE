namespace MovieWebApp.Services
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using MovieWebApp.Models;

    public class OmdbService : IOmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OmdbService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        public async Task<List<MovieSearchResults>> SearchMoviesAsync(string title)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?s={title}&apikey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();

                var searchResults = JsonConvert.DeserializeObject<OmdbSearchResponse>(responseString);

                return searchResults.Search;
            }
            catch(Exception ex)
            {
                throw new Exception("An error occurred while searching for movies.", ex);
            }
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(string imdbId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://www.omdbapi.com/?i={imdbId}&apikey={_apiKey}");
                response.EnsureSuccessStatusCode();
                var movieDetails = JsonConvert.DeserializeObject<MovieDetails>(await response.Content.ReadAsStringAsync());

                if(movieDetails == null || movieDetails.Response != "True")
                {
                    return new MovieDetails
                    {
                        Response = "False",
                    };
                }

                return movieDetails;
            }
            catch (HttpRequestException e)
            {
                throw new Exception("Network error occurred.", e);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while getting movie details.{imdbId}", ex);
            }
        }
    }
}
