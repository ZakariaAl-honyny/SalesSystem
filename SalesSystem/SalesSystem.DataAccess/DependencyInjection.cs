using Microsoft.Extensions.DependencyInjection;
using SalesSystem.Business.Interfaces.Repositories;
using SalesSystem.Business.Interfaces.Services;
using SalesSystem.DataAccess.Repositories;
using SalesSystem.DataAccess.Services;

namespace SalesSystem.DataAccess;

public static class DependencyInjection
{
    /// <summary>
    /// Registers all Infrastructure-layer services including image storage,
    /// and future cross-cutting services.
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<ISystemLogRepository, SystemLogRepository>();

        // LocalImageStorageService holds no scoped state (_basePath is static, ILogger is singleton)
        services.AddSingleton<ILocalImageStorageService, LocalImageStorageService>();

        return services;
    }
}
