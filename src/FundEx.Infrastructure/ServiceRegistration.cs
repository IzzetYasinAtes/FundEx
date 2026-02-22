namespace FundEx.Infrastructure;
using System.Text;
using FundEx.Application.Common.Interfaces;
using FundEx.Infrastructure.BackgroundServices;
using FundEx.Infrastructure.DataSync;
using FundEx.Infrastructure.Caching;
using FundEx.Infrastructure.ExternalServices.Tefas;
using FundEx.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TefasApiSettings>(configuration.GetSection(TefasApiSettings.SectionName));
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddHttpClient<ITefasApiService, TefasApiService>(client =>
        {
            var baseUrl = configuration.GetSection(TefasApiSettings.SectionName)["BaseUrl"]
                ?? "https://tefas.takasbank.com.tr/api/funds/";
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        })
        .AddPolicyHandler(GetRetryPolicy());

        services.AddMemoryCache();
        services.AddSingleton<ICacheService, InMemoryCacheService>();
        services.AddSingleton<JwtTokenService>();
        services.AddScoped<IMetadataSyncService, MetadataSyncService>();
        services.AddScoped<IDailyDataSyncService, DailyDataSyncService>();
        services.AddScoped<DataSyncOrchestrator>();
        services.AddHostedService<DataSyncBackgroundService>();

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
        if (jwtSettings is not null)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
        }

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
