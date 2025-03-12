using System.Reflection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace cash.hub.authentication.api.infra.OpenTelemetry;

public static class OpenTelemetryConfiguration
{
    public static void AddInstrumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var appVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
        var serviceName = $"cash.hub.authentication.api";
        var opentelemetryConfiguration = configuration.GetSection("OpenTelemetry:EndPoint").Get<string>();

        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(serviceName, serviceVersion: appVersion);
            })
            .WithTracing(tracing  =>
            {

                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = new Uri(opentelemetryConfiguration ?? "Default"));
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddOtlpExporter(options => options.Endpoint = new Uri(opentelemetryConfiguration ?? "Default"));
            });
    }
}