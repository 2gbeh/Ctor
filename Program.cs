using Microsoft.OpenApi.Models;
using Ctor.Lib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Babago API",
        Description = "Dispatch & Logistics Services",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Babago API V1");
    });
}

app.UseHttpsRedirection();

var api = app.MapGroup("/api");
api.MapGet("/weather-forecast", WeatherForecast.Handle).WithName("GetWeatherForecast");

app.Run();

