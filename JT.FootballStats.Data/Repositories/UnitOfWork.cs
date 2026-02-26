using JT.FootballStats.Data.Context;

namespace JT.FootballStats.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly FootballStatsContext _context;
    private IRepository<League>? _leagues;
    private IRepository<Team>? _teams;
    private IRepository<Standing>? _standings;

    public UnitOfWork(FootballStatsContext context)
    {
        _context = context;
    }

    public IRepository<League> Leagues => _leagues ??= new Repository<League>(_context);
    public IRepository<Team> Teams => _teams ??= new Repository<Team>(_context);
    public IRepository<Standing> Standings => _standings ??= new Repository<Standing>(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
