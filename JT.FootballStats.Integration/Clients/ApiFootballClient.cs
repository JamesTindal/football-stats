namespace JT.FootballStats.Ingestion.Clients;

public class ApiFootballClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ApiFootballClient(HttpClient httpClient, IOptions<ApiFootballConfig> settings)
    {
        _httpClient = httpClient;
        _apiKey = settings.Value.ApiKey;
        _httpClient.BaseAddress = new Uri("https://v3.football.api-sports.io");
        _httpClient.DefaultRequestHeaders.Add("x-apisports-key", _apiKey);
    }

    public async Task<HttpResponseMessage> GetCurrentPremierLeagueStandingsAsync()
    {
        var leagueId = 39;
        var season = "2023";
        var endpoint = $"/standings?league={leagueId}&season={season}";
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return response;
    }
}
