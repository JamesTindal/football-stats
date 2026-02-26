namespace JT.FootballStats.Tests.Ingestion.IntegrationTests;

public class ApiFootballClientIngestionTests(IngestionTestFixture fixture) : IClassFixture<IngestionTestFixture>
{
    private readonly IngestionTestFixture _fixture = fixture;

    [Fact]
    public async Task GetPremierLeagueTableAsync_ReturnsApiResponse()
    {
        // Arrange
        var client = _fixture.ServiceProvider.GetRequiredService<ApiFootballClient>();

        // Act
        var response = await client.GetCurrentPremierLeagueStandingsAsync();

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Response);
        Assert.NotEmpty(response.Response);
    }
}