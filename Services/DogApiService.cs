using System.Text.Json;
using DogBreedApi.Models;

namespace DogBreedApi.Services
{
    public class DogApiService
    {
        private readonly HttpClient _http;
        private readonly ILogger<DogApiService> _logger;

        private const string BaseUrl = "https://api.thedogapi.com/v1";
        private const string ApiKey  = "live_vMpIZQ0GuKHoXJxEhq01P4d4j2Nr8j2GHuX3qNBq7wCf0kcb9nDq52XsxvBDFTNA";

        private List<Breed>? _cachedBreeds;
        private DateTime _cacheTime = DateTime.MinValue;
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public DogApiService(HttpClient http, ILogger<DogApiService> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<Breed>> GetAllBreedsAsync()
        {
            if (_cachedBreeds != null && DateTime.UtcNow - _cacheTime < CacheDuration)
                return _cachedBreeds;

            try
            {
                _http.DefaultRequestHeaders.Remove("x-api-key");
                _http.DefaultRequestHeaders.Add("x-api-key", ApiKey);

                var response = await _http.GetAsync($"{BaseUrl}/breeds?limit=200");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var breeds = JsonSerializer.Deserialize<List<Breed>>(json, JsonOpts) ?? new();

                _cachedBreeds = breeds;
                _cacheTime = DateTime.UtcNow;
                return breeds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch breeds");
                return _cachedBreeds ?? new();
            }
        }

        public async Task<Breed?> GetBreedByIdAsync(int id)
        {
            var all = await GetAllBreedsAsync();
            return all.FirstOrDefault(b => b.Id == id);
        }

        public async Task<List<Breed>> SearchBreedsAsync(string query)
        {
            try
            {
                _http.DefaultRequestHeaders.Remove("x-api-key");
                _http.DefaultRequestHeaders.Add("x-api-key", ApiKey);

                var response = await _http.GetAsync($"{BaseUrl}/breeds/search?q={Uri.EscapeDataString(query)}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Breed>>(json, JsonOpts) ?? new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search failed");
                return new();
            }
        }

        public async Task<List<Breed>> GetBreedsByGroupAsync(string group)
        {
            var all = await GetAllBreedsAsync();
            return all.Where(b => b.BreedGroup?.Equals(group, StringComparison.OrdinalIgnoreCase) ?? false).ToList();
        }

        public async Task<List<string>> GetBreedGroupsAsync()
        {
            var all = await GetAllBreedsAsync();
            return all.Where(b => !string.IsNullOrEmpty(b.BreedGroup))
                      .Select(b => b.BreedGroup!)
                      .Distinct().OrderBy(g => g).ToList();
        }
    }
}