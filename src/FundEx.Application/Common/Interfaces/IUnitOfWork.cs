namespace FundEx.Application.Common.Interfaces;
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
