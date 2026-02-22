namespace FundEx.Persistence.Repositories;
using FundEx.Application.Common.Interfaces;
using FundEx.Persistence.Contexts;

public class UnitOfWork : IUnitOfWork
{
    private readonly FundExDbContext _context;

    public UnitOfWork(FundExDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
