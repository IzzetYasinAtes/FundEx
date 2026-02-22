namespace FundEx.Persistence.Contexts;
using FundEx.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class FundExDbContext : DbContext
{
    public FundExDbContext(DbContextOptions<FundExDbContext> options) : base(options) { }

    public DbSet<FundType> FundTypes => Set<FundType>();
    public DbSet<FundCategory> FundCategories => Set<FundCategory>();
    public DbSet<FundTitle> FundTitles => Set<FundTitle>();
    public DbSet<Founder> Founders => Set<Founder>();
    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<FundDailyData> FundDailyData => Set<FundDailyData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FundExDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<FundEx.Domain.Common.Entity<Guid>>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedDate = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
