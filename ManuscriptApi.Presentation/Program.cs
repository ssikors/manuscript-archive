using FluentValidation;
using ManuscriptApi.Business.MediatR.Commands;
using ManuscriptApi.Business.MediatR;
using ManuscriptApi.Presentation.Extensions;
using MediatR;
using Serilog;
using System.Data;
using Microsoft.Data.SqlClient;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Information()
    .CreateLogger();

try
{
    Log.Information("Starting up the Manuscript API...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();

    // Extension method
    builder.Services.AddSwaggerDocumentation();

    // Health checks
    builder.Services.AddHealthChecks()
        .AddSqlServer(
            connectionString: builder.Configuration.GetConnectionString("DefaultConnection")!,
            name: "sql",
            failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
            tags: new[] { "db", "sql" }
        );

    // MediatR
    builder.Services.AddMediatR(config =>
    {
        config.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly);
    });

    // Validation
    builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


    // Authentication extension
    builder.Services.AddJwtAuthentication(builder.Configuration);

    // Automapper
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    // Db connection
    builder.Services.AddScoped<IDbConnection>(sp =>
        new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Services
    builder.Services.AddRepositories();
    builder.Services.AddCrudServices();


    var app = builder.Build();

    // Exception handler extension
    app.UseGlobalExceptionHandler();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    Log.Information("Application started successfully");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start");
}
finally
{
    Log.CloseAndFlush();
}
