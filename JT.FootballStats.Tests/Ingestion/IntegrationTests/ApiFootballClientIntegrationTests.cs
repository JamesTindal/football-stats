namespace JT.FootballStats.Tests.Ingestion.IntegrationTests;

public class ApiFootballClientIngestionTests : IClassFixture<IngestionTestFixture>
{
    private readonly IngestionTestFixture _fixture;

    public ApiFootballClientIngestionTests(IngestionTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetPremierLeagueTableAsync_ReturnsJson()
    {
        // Arrange
        var client = _fixture.ServiceProvider.GetRequiredService<ApiFootballClient>();

        // Act
        var response = await client.GetCurrentPremierLeagueStandingsAsync();
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(content);
    }
}