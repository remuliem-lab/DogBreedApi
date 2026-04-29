using System.Text.Json.Serialization;

namespace DogBreedApi.Models
{
    public class Breed
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("breed_group")]
        public string? BreedGroup { get; set; }

        [JsonPropertyName("temperament")]
        public string? Temperament { get; set; }

        [JsonPropertyName("origin")]
        public string? Origin { get; set; }

        [JsonPropertyName("life_span")]
        public string? LifeSpan { get; set; }

        [JsonPropertyName("bred_for")]
        public string? BredFor { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("weight")]
        public Measurement? Weight { get; set; }

        [JsonPropertyName("height")]
        public Measurement? Height { get; set; }

        [JsonPropertyName("image")]
        public BreedImage? Image { get; set; }
    }

    public class Measurement
    {
        [JsonPropertyName("imperial")]
        public string? Imperial { get; set; }

        [JsonPropertyName("metric")]
        public string? Metric { get; set; }
    }

    public class BreedImage
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
