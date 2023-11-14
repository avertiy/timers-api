using AVS.MultiExchange.DataLoaderApi;
using timers_api;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

builder.AddWebApiInfrastructure();
builder.AddAppServices();
builder.Services.AddHostedService<HostedService>();

var corsPolicy = builder.AddCorsAllowAnyOrigin();
var app = builder.Build();
app.ConfigureAppServices(corsPolicy);

app.Run();
