using Newtonsoft.Json;

namespace MovieWebApp.Models
{
    public class OmdbSearchResponse
    {
        [JsonProperty("Search")]
        public List<MovieSearchResults> Search { get; set; }
    }
}
