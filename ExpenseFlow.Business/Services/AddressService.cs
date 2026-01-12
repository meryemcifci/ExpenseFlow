using ExpenseFlow.Business.Abstract;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExpenseFlow.Business.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            if (!_httpClient.DefaultRequestHeaders.UserAgent.Any())
            {
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ExpenseFlowApp/1.0");
            }
        }

        public async Task<List<string>> GetCountriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://countriesnow.space/api/v0.1/countries");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CountriesResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Data?.Select(c => c.Country).OrderBy(c => c).ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCountries Error: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> GetCitiesByCountryAsync(string country)
        {
            try
            {
                var requestBody = new { country = country };
                var jsonContent = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    "https://countriesnow.space/api/v0.1/countries/cities",
                    httpContent);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CitiesResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Data?.OrderBy(c => c).ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetCities Error: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<List<string>> GetDistrictsAsync(string country, string city)
        {
            try
            {
                var requestBody = new { country = country, state = city };
                var jsonContent = JsonSerializer.Serialize(requestBody);
                var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    "https://countriesnow.space/api/v0.1/countries/state/cities",
                    httpContent);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<DistrictsResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.Data?.OrderBy(d => d).ToList() ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetDistricts Error: {ex.Message}");
                return new List<string>();
            }
        }
    }

    // Response Models
    public class CountriesResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public List<CountryData> Data { get; set; }
    }

    public class CountryData
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class CitiesResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public List<string> Data { get; set; }
    }

    public class DistrictsResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

        [JsonPropertyName("data")]
        public List<string> Data { get; set; }
    }
}

