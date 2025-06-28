using System.Text.Json;

namespace Tools;

public class HttpJsonClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public HttpJsonClient(HttpClient httpClient, JsonSerializerOptions? options = null)
    {
        _httpClient = httpClient;
        _jsonOptions = options ?? new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString |
                             System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    /// <summary>
    /// Generalized helper to GET a URL and deserialize JSON response to T.
    /// </summary>
    public async Task<T?> GetAndDeserializeAsync<T>(string url)
    {
        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
    }
}