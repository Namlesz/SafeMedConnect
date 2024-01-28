using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;

namespace SafeMedConnect.Api.Startup;

internal static class RegisterSerilog
{
    public static void ConfigureAndUseSerilog(this ConfigureHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .WriteTo.File(
                path: "Logs/app-.log",
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Warning
            )
            .WriteTo.ApplicationInsights(
                TelemetryConfiguration.CreateDefault(),
                TelemetryConverter.Traces
            )
            .CreateLogger();

        host.UseSerilog();
    }
}