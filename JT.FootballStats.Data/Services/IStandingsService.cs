using JT.FootballStats.Core.DTOs;

namespace JT.FootballStats.Data.Services;

public interface IStandingsService
{
    Task SaveStandingsAsync(ApiStandingsResponse apiResponse);
}
