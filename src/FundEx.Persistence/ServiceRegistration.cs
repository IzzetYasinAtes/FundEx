namespace FundEx.Persistence;
using FundEx.Application.Common.Interfaces;
using FundEx.Persistence.Contexts;
using FundEx.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FundExDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("FundExDb"),
                b => b.MigrationsAssembly(typeof(FundExDbContext).Assembly.FullName)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
