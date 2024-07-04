using Microsoft.AspNetCore.Mvc;
using MovieWebApp.Models;
using MovieWebApp.Services;
using Newtonsoft.Json;

namespace MovieWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IOmdbService _omdbService;
        private readonly ILogger<MovieController> _logger;

        // Temporary storage for recent searches. Would be replaced with a database in a real-world application
        // especially if a requirement for different users would arise.
        private static List<string> _recentSearches = new List<string>();

        public MovieController(IOmdbService omdbService, ILogger<MovieController> logger)
        { 
            _omdbService = omdbService;
            _logger = logger;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(string title)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return BadRequest("Title cannot be empty.");
                }

                if (!_recentSearches.Contains(title))
                {
                    if (_recentSearches.Count >= 5)
                    {
                        _recentSearches.RemoveAt(0);
                    }
                    _recentSearches.Add(title);
                }

                List<MovieSearchResults> result = await _omdbService.SearchMoviesAsync(title);

                if(result == null || result.Count == 0)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching for movies.");
                return StatusCode(500);
            }
        }

        [HttpGet("details")]
        public async Task<IActionResult> GetMovieDetails(string imdbId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imdbId))
                {
                    return BadRequest("ImdbId cannot be empty.");
                }

                MovieDetails response = await _omdbService.GetMovieDetailsAsync(imdbId);

                if(response.Response == "False")
                {
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting movie details.");
                return StatusCode(500);
            }
        }

        [HttpGet("recent-searches")]
        public IActionResult GetRecentSearches()
        {
            try
            {
                return Ok(_recentSearches);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting recent searches.");
                return StatusCode(500);
            }
        }

        [HttpDelete("clear-recent-searches")]
        public void ClearRecentSearches()
        {
            _recentSearches.Clear();
        }
    }
}
