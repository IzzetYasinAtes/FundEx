namespace FundEx.Persistence.Repositories;
using FundEx.Application.Common.Interfaces;
using FundEx.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly FundExDbContext Context;
    protected readonly DbSet<T> DbSet;

    public Repository(FundExDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await DbSet.FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default) =>
        await DbSet.ToListAsync(ct);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await DbSet.Where(predicate).ToListAsync(ct);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await DbSet.FirstOrDefaultAsync(predicate, ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await DbSet.AddAsync(entity, ct);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default) =>
        await DbSet.AddRangeAsync(entities, ct);

    public void Update(T entity) => DbSet.Update(entity);

    public void Remove(T entity) => DbSet.Remove(entity);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default) =>
        await DbSet.AnyAsync(predicate, ct);

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default) =>
        predicate is null ? await DbSet.CountAsync(ct) : await DbSet.CountAsync(predicate, ct);

    public IQueryable<T> Query() => DbSet.AsQueryable();
}
