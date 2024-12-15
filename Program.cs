using Serilog;
using Serilog.Sinks.Loki; // Required for Loki integration

var logger = new LoggerConfiguration()
    .WriteTo.Console() // Log to console
    .WriteTo.LokiHttp("http://loki:3100") // Loki endpoint
    .CreateLogger();

// Use Serilog as the logging provider
Log.Logger = logger;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog to the app's logging pipeline
    builder.Host.UseSerilog();

    var app = builder.Build();

    app.MapGet("/", () =>
    {
        Log.Information("Received request to root endpoint");
        return "Hello, World! From C# K3s Deployment";
    });

    logger.Information("Application starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.Information("Application shutting down");
    Log.CloseAndFlush();
}
