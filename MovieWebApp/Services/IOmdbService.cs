namespace MovieWebApp.Services
{
    using MovieWebApp.Models;
    public interface IOmdbService
    {
        Task<List<MovieSearchResults>> SearchMoviesAsync(string title);
        Task<MovieDetails> GetMovieDetailsAsync(string imdbId);
    }
}
