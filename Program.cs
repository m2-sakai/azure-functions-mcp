using AzureFunctionsMcp;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
builder.EnableMcpToolMetadata();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();
builder.ConfigureMcpTool(nameof(MyMcpTools.GetCurrentTime))
    .WithProperty("timezone", "string", "The timezone.");

builder.Build().Run();
