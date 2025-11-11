using Carter;
using CitizensPortal.Api.Infrastructure.Behaviors;
using CitizensPortal.Api.Infrastructure.Database;
using CitizensPortal.Api.Infrastructure.MultiTenancy;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    // Add services
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "Citizens Portal API",
            Version = "v1",
            Description = "Vertical Slice Architecture with Multi-Tenancy"
        });
    });

    // Multi-tenancy
    builder.Services.AddScoped<ITenantAccessor, TenantAccessor>();

    // Database
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

    // MediatR with behaviors
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(Program).Assembly);

        // Add pipeline behaviors in order
        config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    });

    // FluentValidation - automatically discover all validators in this assembly
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    // Carter - automatically discover all endpoints
    builder.Services.AddCarter();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.WithOrigins(
                    builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "http://localhost:5173" })
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
    });

    var app = builder.Build();

    // Configure middleware pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseCors();

    // Multi-tenancy middleware (must be early in pipeline)
    app.UseMiddleware<TenantMiddleware>();

    app.UseHttpsRedirection();

    // Carter endpoints
    app.MapCarter();

    // Health check
    app.MapGet("/health", () => Results.Ok(new
    {
        Status = "Healthy",
        Timestamp = DateTime.UtcNow
    }))
    .WithName("HealthCheck")
    .WithTags("Health")
    .WithOpenApi();

    Log.Information("Starting Citizens Portal API...");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
