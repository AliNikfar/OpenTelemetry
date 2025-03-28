using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Sinks.Elasticsearch;

//configureSerilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console().CreateLogger();
try
{
    Log.Information("application starts");
    var builder = WebApplication.CreateBuilder(args);


    //This Line have to add to serilog works
    builder.Host.UseSerilog((context,config) =>
    {
        config.MinimumLevel.Information().WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http:\\localhost:1000"))
        {
            AutoRegisterTemplate = true,
            IndexFormat = "ObservabilitySecond-{0:yyy.MM}",
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
        }).Enrich.WithSpan();

    });

    // Add services to the container.
    builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient("ApiFirst", c =>
{
    c.BaseAddress = new Uri("http://localhost:5080/");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    // Configure Serilog for all logs
    //app.UseSerilogRequestLogging();

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

