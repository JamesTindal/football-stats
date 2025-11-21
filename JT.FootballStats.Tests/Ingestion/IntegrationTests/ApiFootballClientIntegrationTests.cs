public class ApiFootballClientIntegrationTests : IClassFixture<IngestionTestFixture>
{
    private readonly IngestionTestFixture _fixture;

    public ApiFootballClientIntegrationTests(IngestionTestFixture fixture)
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