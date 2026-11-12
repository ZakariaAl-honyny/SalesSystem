using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SalesSystem.Business.Infrastructure;
using SalesSystem.DTO.Common;

namespace SalesSystem.DataAccess.Initialization;

public sealed class DatabaseInitializer : IApplicationInitializer
{
    private readonly SalesDbContext _db;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(SalesDbContext db, ILogger<DatabaseInitializer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Result> InitializeAsync(CancellationToken ct = default)
    {
        try
        {
            var pending = (await _db.Database.GetPendingMigrationsAsync(ct)).ToList();

            if (pending.Count > 0)
            {
                if (pending.Count <= 10)
                {
                    _logger.LogInformation("Applying {Count} pending migrations: {Migrations}",
                        pending.Count, string.Join(", ", pending));
                }
                else
                {
                    _logger.LogInformation("Applying {Count} pending migrations", pending.Count);
                }
            }

            await _db.Database.MigrateAsync(ct);

            await DbSeeder.SeedAsync(_db, _logger);

            _logger.LogInformation("Database initialization completed successfully");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database initialization failed");
            return Result.Failure("تعذر تهيئة قاعدة البيانات. تحقق من الاتصال ثم أعد تشغيل النظام.");
        }
    }
}
