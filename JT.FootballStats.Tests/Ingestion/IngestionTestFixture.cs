namespace JT.FootballStats.Tests.Ingestion;

public class IngestionTestFixture
{
    public IServiceProvider ServiceProvider { get; }

    public IngestionTestFixture()
    {
        var configBuilder = new ConfigurationBuilder()
            .AddUserSecrets<IngestionTestFixture>();
        var configuration = configBuilder.Build();

        var services = new ServiceCollection();
        services.Configure<ApiFootballConfig>(
            configuration.GetSection("ApiFootball")
        );
        services.AddHttpClient<ApiFootballClient>();

        ServiceProvider = services.BuildServiceProvider();
    }
}
