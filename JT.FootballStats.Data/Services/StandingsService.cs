using JT.FootballStats.Core.DTOs;
using JT.FootballStats.Data.Repositories;

namespace JT.FootballStats.Data.Services;

public class StandingsService : IStandingsService
{
    private readonly IUnitOfWork _unitOfWork;

    public StandingsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SaveStandingsAsync(ApiStandingsResponse apiResponse)
    {
        if (apiResponse?.Response == null || apiResponse.Response.Count == 0)
            return;

        foreach (var leagueResponse in apiResponse.Response)
        {
            var apiLeague = leagueResponse.League;
            if (apiLeague?.Standings == null || apiLeague.Standings.Count == 0)
                continue;

            // Upsert League
            var league = await _unitOfWork.Leagues.GetByIdAsync(apiLeague.Id);
            if (league == null)
            {
                league = new League
                {
                    Id = apiLeague.Id,
                    Name = apiLeague.Name,
                    Season = apiLeague.Season,
                    Standings = []
                };
                await _unitOfWork.Leagues.AddAsync(league);
            }
            else
            {
                league.Name = apiLeague.Name;
                league.Season = apiLeague.Season;
                _unitOfWork.Leagues.Update(league);
            }

            var standingEntries = apiLeague.Standings.FirstOrDefault();
            if (standingEntries == null || standingEntries.Count == 0)
                continue;

            foreach (var entry in standingEntries)
            {
                var team = await _unitOfWork.Teams.GetByIdAsync(entry.Team.Id);
                if (team == null)
                {
                    team = new Team
                    {
                        Id = entry.Team.Id,
                        Name = entry.Team.Name
                    };
                    await _unitOfWork.Teams.AddAsync(team);
                }
                else
                {
                    team.Name = entry.Team.Name;
                    _unitOfWork.Teams.Update(team);
                }

                var standings = await _unitOfWork.Standings.FindAsync(s => 
                    s.LeagueId == apiLeague.Id && s.TeamId == entry.Team.Id);
                
                var standing = standings.FirstOrDefault();
                
                if (standing == null)
                {
                    standing = new Standing
                    {
                        LeagueId = apiLeague.Id,
                        TeamId = entry.Team.Id,
                        League = league,
                        Team = team,
                        Rank = entry.Rank,
                        Points = entry.Points,
                        Played = entry.All.Played,
                        Won = entry.All.Win,
                        Drawn = entry.All.Draw,
                        Lost = entry.All.Lose,
                        GoalsFor = entry.All.Goals.For,
                        GoalsAgainst = entry.All.Goals.Against,
                        UpdatedAt = entry.Update
                    };
                    await _unitOfWork.Standings.AddAsync(standing);
                }
                else
                {
                    standing.Rank = entry.Rank;
                    standing.Points = entry.Points;
                    standing.Played = entry.All.Played;
                    standing.Won = entry.All.Win;
                    standing.Drawn = entry.All.Draw;
                    standing.Lost = entry.All.Lose;
                    standing.GoalsFor = entry.All.Goals.For;
                    standing.GoalsAgainst = entry.All.Goals.Against;
                    standing.UpdatedAt = entry.Update;
                    _unitOfWork.Standings.Update(standing);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
