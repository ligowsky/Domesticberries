using BitzArt;
using MassTransit.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Dberries;

public static class AddTelemetryExtension
{
    public static WebApplicationBuilder AddTelemetry(this WebApplicationBuilder builder)
    {
        var options =
            DberriesApplicationOptions.Get<ElasticApmOptions>(builder.Services, builder.Configuration, "ElasticApm");

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(options.ServiceName)
            .AddTelemetrySdk()
            .AddAttributes(
                new KeyValuePair<string, object>[]
                {
                    new("deployment.environment", options.Environment)
                })
            .AddEnvironmentVariableDetector();

        builder.Logging
            .AddOpenTelemetry(x =>
            {
                x.SetResourceBuilder(resourceBuilder)
                    .AddOtlpExporter(cfg => { cfg.Endpoint = new Uri(options.ServerUrl); });

                x.IncludeFormattedMessage = true;
                x.IncludeScopes = true;
                x.ParseStateValues = true;
            });

        builder.Services.AddOpenTelemetry()
            .WithTracing(trace =>
            {
                trace
                    .AddSource(DiagnosticHeaders.DefaultListenerName)
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation(o => { o.RecordException = true; })
                    .AddSqlClientInstrumentation(o =>
                    {
                        o.EnableConnectionLevelAttributes = true;
                        o.RecordException = true;
                        o.SetDbStatementForText = true;
                        o.SetDbStatementForStoredProcedure = true;
                    })
                    .AddHttpClientInstrumentation(o =>
                    {
                        o.RecordException = true;

                        if (options.EnrichOutboundHttpRequests)
                        {
                            o.EnrichWithHttpRequestMessage = HttpClientEnrichUtility.EnrichWithHttpRequestMessage;
                            o.EnrichWithHttpResponseMessage = HttpClientEnrichUtility.EnrichWithHttpResponseMessage;
                        }
                    })
                    .AddOtlpExporter(cfg => { cfg.Endpoint = new Uri(options.ServerUrl); });
            });

        ExceptionTelemetry.EnableOpenTelemetry();

        return builder;
    }
}