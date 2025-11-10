using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using CitizensPortal.EntityFrameworkCore;

namespace CitizensPortal.DbMigrator;

public class CitizensPortalDbMigrationService : ITransientDependency
{
    private readonly ILogger<CitizensPortalDbMigrationService> _logger;
    private readonly IDataSeeder _dataSeeder;
    private readonly CitizensPortalDbContext _dbContext;
    private readonly ICurrentTenant _currentTenant;

    public CitizensPortalDbMigrationService(
        ILogger<CitizensPortalDbMigrationService> logger,
        IDataSeeder dataSeeder,
        CitizensPortalDbContext dbContext,
        ICurrentTenant currentTenant)
    {
        _logger = logger;
        _dataSeeder = dataSeeder;
        _dbContext = dbContext;
        _currentTenant = currentTenant;
    }

    public async Task MigrateAsync()
    {
        var initialMigrationAdded = AddInitialMigrationIfNotExist();

        if (initialMigrationAdded)
        {
            return;
        }

        _logger.LogInformation("Started database migrations...");

        await MigrateDatabaseSchemaAsync();
        await SeedDataAsync();

        _logger.LogInformation("Successfully completed database migrations.");
        _logger.LogInformation("You can safely close the console window.");
    }

    private async Task MigrateDatabaseSchemaAsync()
    {
        _logger.LogInformation($"Migrating schema for host database...");

        var stopwatch = Stopwatch.StartNew();
        await _dbContext.Database.MigrateAsync();

        stopwatch.Stop();
        _logger.LogInformation($"Completed host database schema migration in {stopwatch.Elapsed.TotalSeconds:0.00} seconds.");
    }

    private async Task SeedDataAsync()
    {
        _logger.LogInformation("Executing host database seed...");

        await _dataSeeder.SeedAsync();

        _logger.LogInformation("Successfully completed host database seed.");
    }

    private bool AddInitialMigrationIfNotExist()
    {
        try
        {
            if (!_dbContext.Database.GetPendingMigrations().Any())
            {
                return false;
            }

            if (!_dbContext.Database.GetMigrations().Any())
            {
                _logger.LogError(
                    "No migrations found! Please add initial migration using: " +
                    "cd src/CitizensPortal.EntityFrameworkCore && dotnet ef migrations add InitialCreate"
                );
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking database migrations.");
        }

        return false;
    }
}
