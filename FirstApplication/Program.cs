using FirstApplication.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Elasticsearch;
using OpenTelemetry.Extensions.Hosting;

//configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console().CreateLogger();
try
{
    Log.Information("application starts");
    var builder = WebApplication.CreateBuilder(args);


    //This Line have to add to serilog works
    builder.Host.UseSerilog((context, config) => 
    {
        config.MinimumLevel.Information().WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
        {
            AutoRegisterTemplate = true,
            IndexFormat = "ObservabilitySecond-{0:yyy.MM}",
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
        }).Enrich.WithSpan(); ;
    });
    
    // After install Opentelemetry.instruments and opentelemtry.export configure opentelemetry
    var serviceName = "ObservabilityFirst";
    var serviceVersion = "1.0.0";

    builder.Services.AddOpenTelemetryTracing(tracerProviderBuilder =>
    {
        tracerProviderBuilder
        .AddConsoleExporter()
        .AddJaegerExporter()
        .AddSource(serviceName)
        .AddResourceBuilder(
            ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddSqlClientInstrumentation();
    });


    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddDbContext<PersonDbContext>();
    builder.Services.AddScoped<PersonRepo>();
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    // Configure Serilog for all logs
    app.UseSerilogRequestLogging();

    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch
{
    Log.Error("application Error");
}
finally
{
    Log.CloseAndFlush();
}

